using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        var query = from s in DaneUczelni.Studenci
            where s.Miasto.Equals("Warsaw")
            select $"{s.NumerIndeksu}, {s.Imie}, {s.Nazwisko}, {s.Miasto}";
        return query;
    }

    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        
            var query = from s in DaneUczelni.Studenci
                select s.Email;
            return query;
        }

    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
            var query = from s in DaneUczelni.Studenci
                orderby s.Nazwisko, s.Imie
                select $"{s.NumerIndeksu}, {s.Imie} {s.Nazwisko}";

            return query;
    }

    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var przedmiot = DaneUczelni.Przedmioty
            .FirstOrDefault(p => p.Kategoria == "Analytics");

        if (przedmiot == null)
            return ["Nie ma żadnego przedmiotu."];

        return [$"{przedmiot.Nazwa}, {przedmiot.DataStartu:yyyy-MM-dd}"];
    }

    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        bool zapis = DaneUczelni.Zapisy.Any(z => !z.CzyAktywny);

        return [$"Nieaktywny zapis istnieje: {zapis}"];
    }

    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        bool prowadzący = DaneUczelni.Prowadzacy
            .All(p => !string.IsNullOrWhiteSpace(p.Katedra));

        return [$"Czy każdy ma przypisaną katedrę? {prowadzący}"];
    }

    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        int policz = DaneUczelni.Zapisy
            .Count(z => z.CzyAktywny);

        return [$"Aktywnych zapisów jest: {policz}"];
    }

    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .OrderBy(m => m);
    }

    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu:yyyy-MM-dd}, {z.StudentId}, {z.PrzedmiotId}");
    }

        public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
        {
            return (from p in DaneUczelni.Przedmioty
                    orderby p.Nazwa
                    select $"{p.Nazwa}, {p.Kategoria}")
                .Skip(2)
                .Take(2);
        }

        public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
        {
            return from s in DaneUczelni.Studenci
                join z in DaneUczelni.Zapisy on s.Id equals z.StudentId
                select $"{s.Imie} {s.Nazwisko}, {z.DataZapisu:yyyy-MM-dd}";
        }

        public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
        {
            return from z in DaneUczelni.Zapisy
                join s in DaneUczelni.Studenci on z.StudentId equals s.Id
                join p in DaneUczelni.Przedmioty on z.PrzedmiotId equals p.Id
                select $"{s.Imie} {s.Nazwisko} - {p.Nazwa}";
        }

        public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
        {
            return from z in DaneUczelni.Zapisy
                join p in DaneUczelni.Przedmioty on z.PrzedmiotId equals p.Id
                group p by p.Nazwa into g
                select $"{g.Key}: {g.Count()}";
        }

        public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
        {
            return from z in DaneUczelni.Zapisy
                where z.OcenaKoncowa != null
                join p in DaneUczelni.Przedmioty on z.PrzedmiotId equals p.Id
                group z.OcenaKoncowa by p.Nazwa into g
                select $"{g.Key}, {g.Average()}";
        }

    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, przedmioty) =>
                    $"{pr.Imie} {pr.Nazwisko}, {przedmioty.Count()}");
    }

    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa != null)
            .Join(DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => new { s.Imie, s.Nazwisko, z.OcenaKoncowa})
            .GroupBy(x => new {x.Imie, x.Nazwisko})
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko}, {g.Max(x => x.OcenaKoncowa)}");
    }

    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return from s in DaneUczelni.Studenci
            join z in DaneUczelni.Zapisy on s.Id equals z.StudentId
            where z.CzyAktywny
            group z by new { s.Imie, s.Nazwisko } into g
            where g.Count() > 1
            select $"{g.Key.Imie} {g.Key.Nazwisko}, {g.Count()}";
    }

    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return from p in DaneUczelni.Przedmioty
            join z in DaneUczelni.Zapisy on p.Id equals z.PrzedmiotId into zapisyPrzedmiotu
            where p.DataStartu.Month == 4 && p.DataStartu.Year == 2026
            where zapisyPrzedmiotu.All(z => z.OcenaKoncowa == null)
            select p.Nazwa;
    }

    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        return DaneUczelni.Prowadzacy.Select(pr => {
            var oceny = DaneUczelni.Przedmioty
                .Where(p => p.ProwadzacyId == pr.Id)
                .Join(DaneUczelni.Zapisy, p => p.Id, z => z.PrzedmiotId, (p, z) => z.OcenaKoncowa)
                .Where(o => o != null);

            var srednia = oceny.Any() ? oceny.Average()?.ToString() : "brak ocen";
            return $"{pr.Imie} {pr.Nazwisko}, {srednia}";
        });
    }

    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .Join(DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => s.Miasto)
            .GroupBy(m => m)
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key}, {g.Count()}");
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
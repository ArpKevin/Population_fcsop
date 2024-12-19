using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Population
{
    internal class Allampolgar
    {
        public int Id { get; set; }
        public string Nem { get; set; }
        public DateTime SzuletesiDatum { get; set; }
        public string CsaladiAllapot { get; set; }
        public bool Egeszsegbiztositas { get; set; }
        public string EgeszsegbiztositasSzovegesen { get; set; }
        public string EgeszsegugyiEllatas { get; set; }
        public string EgeszsegugyiEllatasSzovegesen { get; set; }
        public string Nemzetiseg { get; set; }
        public string Megye { get; set; }
        public double HaviBruttoJovedelem { get; set; }
        public double EvesEgyebBruttoJovedelem { get; set; }
        public string IskolaiVegzettseg { get; set; }
        public string IskolaiVegzettsegSzovegesen { get; set; }
        public string PolitikaiNezet { get; set; }
        public bool Szavazokepes { get; set; }
        public int TeaFogyasztasNaponta { get; set; }
        public int FishAndChipsFogyasztasEvente { get; set; }
        public string FishAndChipsFogyasztasEventeSzovegesen { get; set; }

        public Allampolgar(string adatSor)
        {
            var temp = adatSor.Split(';');
            Id = int.Parse(temp[0]);
            Nem = temp[1];
            SzuletesiDatum = DateTime.Parse(temp[2]).Date;
            CsaladiAllapot = temp[3];
            EgeszsegbiztositasSzovegesen = temp[4];
            Egeszsegbiztositas = temp[4] == "van";
            EgeszsegugyiEllatas = string.IsNullOrWhiteSpace(temp[5]) ? string.Empty : temp[5];
            EgeszsegugyiEllatasSzovegesen = string.IsNullOrWhiteSpace(temp[5]) ? "-" : temp[5];
            Nemzetiseg = temp[6];
            Megye = temp[7];
            HaviBruttoJovedelem = double.TryParse(temp[8], out double havi) ? havi : -1;
            EvesEgyebBruttoJovedelem = double.Parse(temp[9]);
            IskolaiVegzettseg = string.IsNullOrWhiteSpace(temp[10]) ? string.Empty : temp[10];
            IskolaiVegzettsegSzovegesen = string.IsNullOrWhiteSpace(temp[10]) ? "-" : temp[10];
            PolitikaiNezet = temp[11];
            Szavazokepes = Szavazokepesseg();
            TeaFogyasztasNaponta = int.Parse(temp[13]);
            FishAndChipsFogyasztasEvente = temp[14] == "?" ? -1 : int.Parse(temp[14]);
            FishAndChipsFogyasztasEventeSzovegesen = temp[14];
        }

        public bool Szavazokepesseg()
        {
            return DateTime.Now > SzuletesiDatum.AddYears(21) ? true : false;
        }

        public double TeljesJovedelem()
        {
            return HaviBruttoJovedelem * 12 + EvesEgyebBruttoJovedelem;
        }

        public string Megjelenit(bool rovid)
        {
            if (rovid)
            {
                return $"{Id}\t{Nem}\t{SzuletesiDatum.ToShortDateString()}\t{CsaladiAllapot}\t{EgeszsegbiztositasSzovegesen}";
            }
            else
            {
                return $"{Id}\t{Nemzetiseg}\t{Megye}\t{HaviBruttoJovedelem:F2}\t{IskolaiVegzettseg}";
            }
        }
    }
}

using BelajarEFCore.Models;
using BelajarEFCore.Data;
using Microsoft.CodeAnalysis;

class Program
{
    static async Task Main(string[] args)
    {
        using (var context = new AppDbContext()) {
            Console.WriteLine("Membuat database...");
            context.Database.EnsureCreated();

            Console.WriteLine("=====Menambahkan Data=======");
            var mashBaru = new Mahasiswa
            {
                Nama = "Aldi wijaya",
                Jurusan = "Teknik Industri",
                TanggalLahir = new DateTime(2001, 3, 15)
            };
            context.Mahasiswa.Add(mashBaru);
            await context.SaveChangesAsync();
            Console.WriteLine("Data Mahasiswa berhasil ditambahkan !");

            var mshList = new List<Mahasiswa>
            {
                new Mahasiswa{Nama = "Andi putra", Jurusan="Sistem Informasi", TanggalLahir = new DateTime(1999, 1, 22)},
                new Mahasiswa{Nama = "Asep komarudin", Jurusan= "Teknik Informatika", TanggalLahir = new DateTime(1992, 8, 21)}
            };
            context.Mahasiswa.AddRange(mshList);
            await context.SaveChangesAsync();
            Console.WriteLine("Data batch telah berhasil ditambahkan");

            Console.WriteLine("====Menampilkan Semua Data=====");
            var semuaMahasiswa = context.Mahasiswa.ToList();

            if (!semuaMahasiswa.Any())
            {
                Console.WriteLine("Tidak ada data Mahasiswa");
            }else
            {
                foreach(var mhs in semuaMahasiswa)
                {
                    Console.WriteLine($"ID: {mhs.Id,-3} | Nama: {mhs.Nama,-8} | Jurusan: {mhs.Jurusan,-4} | Lahir: {mhs.TanggalLahir:dd-MM-yyyy}");
                }
            }

            //read dengan filter
            Console.WriteLine("========Mencari Mahasiswa IT======");
            var mahasiswaIT = context.Mahasiswa
                                .Where(m => m.Jurusan.Contains("Informatika"))
                                .ToList();
            
            foreach( var msh in mahasiswaIT)
            {
                Console.WriteLine($"- {msh.Nama}");
            }
            //UPDATE

            Console.WriteLine("====== Update Data ======");
            var mshPertama = context.Mahasiswa.Where(m=> m.Id == 1).FirstOrDefault();
            if (mshPertama != null)
            {
                System.Console.WriteLine($"Sebelum: {mshPertama.Nama} - {mshPertama.Jurusan}");
                mshPertama.Jurusan = "Teknik Komputer";
                context.Mahasiswa.Update(mshPertama);
                await context.SaveChangesAsync();
                System.Console.WriteLine($"Sesudah: {mshPertama.Nama} - {mshPertama.Jurusan}");
            };

            Console.WriteLine("=======Hapus Data=========");
            var mshTerkahir = context.Mahasiswa.OrderByDescending(m => m.Id).FirstOrDefault();
            if (mshTerkahir != null)
            {
                Console.WriteLine($"Menghapus: {mshTerkahir.Nama}");
                context.Mahasiswa.Remove(mshTerkahir);
                await context.SaveChangesAsync();
                Console.WriteLine("Data Berhasil Dihapus");
            }

            Console.WriteLine("====Data Akhir====");
            var jumlah = context.Mahasiswa.Count();
            Console.WriteLine($"Total Mahasiswa: {jumlah}");
        }





    }
}
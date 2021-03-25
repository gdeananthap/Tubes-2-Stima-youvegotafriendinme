# People You May Know - you'vegotafriendinme
Tugas Besar 2 IF2211 Strategi Algoritma Semester II Tahun 2020/2021 

Pengaplikasian Algoritma BFS dan DFS dalam Fitur People You May Know Jejaring Sosial Facebook

## Description
You'vegotafriendinme merupakan sebuah aplikasi GUI sederhana yang dibuat dengan bahasa pemrograman C# dalam framework .NET yang dapat memodelkan beberapa fitur dari People You May Know dalam jejaring sosial media (Social Network). Dengan memanfaatkan algoritma Breadth First Search (BFS) dan Depth First Search (DFS), Anda dapat menelusuri social network untuk mendapatkan rekomendasi teman seperti pada fitur People You May Know. Selain itu, Anda dapat mendapatkan jalur koneksi untuk dua akun yang belum berteman dan bisa berkenalan melalui jalur tertentu.

## Algorithm
Aplikasi ini memiliki 2 fitur yaitu Friends Recommendation dan Explore Friends yang dapat ditelusuri dengan 2 algoritma yaitu Breadth First Search (BFS) dan Depth First Search (DFS). Pada algoritma pencarian melebar (BFS), semua simpul pada level ke-n akan dikunjungi lebih dahulu sebelum mengunjungi level n+1. Biasanya, BFS dimulai dari simpul dengan “id” terkecil (biasanya 1-n atau A-Z) atau dari simpul paling kiri ke kanan. Proses ini diulangi hingga ditemukan solusi. Adapun algoritma BFS sebagai berikut:
1.	Kunjungi simpul v 
2.	Kunjungi semua simpul yang bertetangga dengan simpul v terlebih dahulu. 
3.	Kunjungi simpul yang belum dikunjungi dan bertetangga dengan simpul -simpul yang tadi dikunjungi, demikian seterusnya.

Sementara, pada algoritma pencarian mendalam (DFS), proses pencarian akan dilakukan pada semua level selanjutnya dari sebuah simpul sebelum dilanjutkan ke simpul-simpul pada level yang sama. Biasanya, BFS dimulai dari simpul dengan “id” terkecil (biasanya 1-n atau A-Z) atau dari simpul paling kiri ke kanan. Proses ini diulangi hingga ditemukan solusi. Adapun algoritma DFS sebagai berikut:
1.	Kunjungi simpul v
2.	Kunjungi simpul w yang bertetangga dengan simpul v.
3.	Ulangi DFS mulai dari simpul w.
4.	Ketika mencapai simpul u sedemikian sehingga semua simpul yang bertetangga dengannya telah dikunjungi, pencarian dirunut-balik (backtrack) ke simpul terakhir yang dikunjungi sebelumnya dan mempunyai simpul w yang belum dikunjungi.
5.	Pencarian berakhir bila tidak ada lagi simpul yang belum dikunjungi yang dapat dicapai dari simpul yang telah dikunjungi.



## Requirements
**Catatan :** Anda perlu melakukan instalasi beberapa hal ini hanya jika Anda ingin menjalankan dari file solution.
1. [Visual Studio](https://visualstudio.microsoft.com/downloads/)
2. [MSAGL](https://github.com/microsoft/automatic-graph-layout)

## How To Run
Sebelum menjalankan aplikasi, pastikan Anda sudah melakukan download atau clone repository ini. Untuk menjalankan aplikasi you'vegotafriendinme dapat dilakukan dengan 2 cara antara lain 
- Via File Solution dengan Visual Studio
   
    Untuk menjalan aplikasi via file solution, Anda dapat melakukan hal berikut :
    
    1.  Jalankan Visual Studio yang sudah anda install sebelumnya.
    2.  Buka repository ini pada Visual Studio.
    3.  Buka solution explorer dan klik pada file Tubes-2-Stima-youvegotafriendinme.sln
    4.  Klik tombol "Play" berwarna hijau pada panel atas dan pastikan Tubes-2-Stima-youvegotafriendinme sudah terpilih.
    5.  Visual studio akan secara otomatis melakukan *build* terhadap aplikasi ini dan jika build berhasil aplikasi akan segera dijalankan.
- Via Executable Code

    Untuk menjalankan aplikasi via executable code, Anda cukup buka folder bin dan double klik pada file Tubes-2-Stima-youvegotafriendinme

## How To Use
1. Jalankan aplikasi you'vegotafriendinme (via file solution atau via executable code)
2. Jika aplikasi berhasil dijalankan, akan ditampilkan menu utama dari aplikasi you’vegotafriendinme.
3. Klik tombol “Browse” untuk memilih file graf yang ingin digunakan. Pastikan format isi file graf sesuai. Contoh format isi file graf yang sesuai ada di folder src/test.
4. Jika format file sesuai, akan ditampilkan visualisasi graf dari file graf yang dimasukkan kedalam aplikasi.
5. Pilih fitur dan algoritma yang ingin Anda gunakan. Kemudian pilih akun-akun yang dibutuhkan untuk menggunakan fitur tersebut. Fitur Friend Recommendation hanya membutuhkan 1 akun yang ada pada graf, sedangkan fitur Explore Friends membutuhkan dua akun yang ada pada graf.
6. Untuk menampilkan hasilnya, klik tombol “Submit”. Hasil akan ditampilkan di kolom “Result”.
7. Jika Anda sudah selesai menggunakan aplikasi, Anda dapat keluar dari aplikasi dengan klik tombo “X” di pojok kanan atas aplikasi.

## Author - you'vegotafriendinme
1. Gde Anantha Priharsena / 13519026
2. Kinantan Arya Bagaspati / 13519043
3. Syarifah Aisha Geubrina Yasmin / 13519089
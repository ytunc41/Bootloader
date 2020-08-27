using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootloader
{
    public static class Versiyon
    {
        private static string _versiyon;
        public static string getVS { get { return _versiyon; } private set { _versiyon = value; } }

        static Versiyon()
        {
            _versiyon = "v1.0.7";
        }


        /* Versiyon: 1.0.7
         * Tarih: 27.08.2020
         * 
         * - FlashSizeTopla() ve UniqueIDTopla() metodlarında düzeltmeler yapilmistir.
         * - Bug fixed.
         * 
         */

        /* Versiyon: 1.0.6
         * Tarih: 27.08.2020
         * 
         * - cmbDataWidth duzenlenmistir.
         * - PACKET_TYPE enum yapisi duzenlenmistir.
         * - Algoritmalar guncelenmistir.
         * - FlashSize ve UniqueID paketleri ve enum yapilari eklenmistir.
         */

        /* Versiyon: 1.0.5
         * Tarih: 27.08.2020
         * 
         * - CHECK_STATUS enum sınıfı eklenip sof1, sof2, crc1, crc2 değerleri sabit bir yerde tutulmustur.
         * - Cihaza gönderilen istek paketi sonucu cihazdan alınan flash verileri "deviceMemory" data yığınında toplanmistir.
         * - Connect, Disconnect ve diger bazi eventlar guncellenmistir.
         * - Device Memory tab file duzenlenmistir.
         * - PaketTopla metodu ve Erase paketi olusturulmustur.
         */

        /* Versiyon: 1.0.4
         * Tarih: 25.08.2020
         * 
         * - SerialPort kütüphanesi eklenmistir.
         * - Paket_Islemleri_LE methodları eklenmistir.(Little Endian islemciler icin) 
         */

        /* Versiyon: 1.0.3
         * Tarih: 25.08.2020
         * 
         * - HidLibrary kutuphanesinin dusuk hizi nedeniyle kaldirilmasi uygun gorulmustur.
         * - Daha yuksek hizdaki cdc kullanilmasi icin seri port baglantisi yapilacaktir.
         * - Haberlesme icin paket yapisi olusturulmustur.
         */

        /* Versiyon: 1.0.2
         * Tarih: 22.08.2020
         * 
         * - Veri yiginina data ekleme algoritmasi guncellenmistir.
         * - List View ile data yiginini yazdirma algoritmasi guncellenmistir.
         */

        /* Versiyon: 1.0.1
         * Tarih: 21.08.2020
         * 
         * - Veri yiginina data ekleme algoritmasi guncellenmistir.
         */

        /* Versiyon: 1.0.0
         * Tarih: 20.08.2020
         * 
         * - Hex dosya acma islemleri yapilmistir.
         * - Hex file parse uygulanmistir.
         * - Data yigini olusturulup veriler sozlukte saklanmistir.
         */

    }
}

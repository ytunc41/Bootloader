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
            getVS = "v1.4.3";
        }

        /* Versiyon: 1.4.3
         * Tarih: 04.09.2020
         * 
         * - Hata duzeltmeleri yapildi.
         */

        /* Versiyon: 1.4.2
         * Tarih: 04.09.2020
         * 
         * - RichTextBox yazdirma islemleri icin invoke metotlari kullanilmistir ve bu sebeple thread cakismasi engellenmistir.
         * - revID ve devID bilgisi kaldirilmistir.
         * - Arayuzun daha kararli calismasi icin duzenlemeler yapilmistir.
         * - Thread'lerin cakismasi onlenmistir.
         * - PAKET_FLAG nesnesi commPro nesnesi icerisine eklenmistir.
         */

        /* Versiyon: 1.4.1
         * Tarih: 02.09.2020
         * 
         * - Duzenlemeler yapilmistir.
         * - CRC32 field'i hexfile nesnesi icerisine tasinmistir.
         */

        /* Versiyon: 1.4.0
         * Tarih: 02.09.2020
         * 
         * - Program verify edildikten sonra program flasha yazilir ve arayuze PROGRAM_OK paketi gelmektedir.
         * - Execute Butonuna basıldıgında PROGRAM_RUN paketi gitmektedir.
         * - Program Checksum bilgisi gonderilmistir.
         * - Checksum dogru ise PROGRAM_OK paketi geri gonderilmis ve kullanici bilgilendirilmistir.
         * - Info textbox icin eklemeler yapilmistir.
         */

        /* Versiyon: 1.3.2
         * Tarih: 02.09.2020
         * 
         * - Cihaz bilgi paketi alirken revID ve devID bilgisi eklenmistir.
         */

        /* Versiyon: 1.3.1
         * Tarih: 02.09.2020
         * 
         * - Device nesnesine revID ve devID property'leri eklenmistir.
         * - SerialPortDetect() metodu Helper nesnesi icerisine eklenmistir.
         * - ST cihaza program atmak icin atilmak istenen program daha once acilmamissa,
         *   butona bastiktan sonra ilk olarak hex dosyasini acip ardindan cihaza yollama ozelligi uygulanmistir.
         * - Program yuklendikten sonra programi calistirmak icin Execute butonu eklenmistir.
         */

        /* Versiyon: 1.3.0
         * Tarih: 02.09.2020
         * 
         * - FLASH_SIZE ve UNIQUE_ID paket türleri silinip BAGLANTI_OK paket türü altında birlestirilmistir.
         * - FlashSizePaketTopla() ve UniqueIDPaketTopla() metotları silinip yerine CahizBilgisiTopla() olusturulmustur.
         */

        /* Versiyon: 1.2.1
         * Tarih: 01.09.2020
         * 
         * - Static Helper nesnesi eklenmistir.
         * - Programda yardimci olacak static metotlar burada tanimlanacaktir.
         * - Information bilgi kutusu listview nesnesinin altına eklenmistir.
         * - Program yukleme, hex file yukleme gibi islemlerdeki durumlar yazdirilmistir.
         */

        /* Versiyon: 1.2.0
         * Tarih: 31.08.2020
         * 
         * - Program gonderme paketi icerisinde bulunan adres bilgisi paket iceriginden silinmistir.
         */

        /* Versiyon: 1.1.1
         * Tarih: 31.08.2020
         * 
         * -ListView yazma islemleri icin yeni bir metot kullanilmistir.
         * 
         */

        /* Versiyon: 1.1.0
         * Tarih: 30.08.2020
         * 
         * - Yeni paket türleri eklenmistir.
         * - Baglanti islemi esnasında hem baglanti paketi hem okuma paketi gonderilmistir. 
         */

        /* Versiyon: 1.0.15
         * Tarih: 29.08.2020
         * 
         * - HexFile ve Device siniflarinda guncellemeler yapilmistir.
         * - SeriPortForm eventlari eklenmistir.
         * - btnConnect ve btnRefresh Click eventlari guncellenmistir.
         * - Program.cs nesnesine eklemeler yapilmistir.
         */

        /* Versiyon: 1.0.14
         * Tarih: 29.08.2020
         * 
         * - HexFile ve Device siniflarinda guncellemeler yapilmistir.
         * - SerialPortDetect metodu guncellenmistir.
         * - btnConnect Click event'i guncellenmistir.
         * - SerialPortForm nesnesi olusturulmustur.
         * - Otomatik olarak bulunamayan ST cihazi icin com secmek amaciyla com'lari listelemek icin kullanilir.
         */

        /* Versiyon: 1.0.13
         * Tarih: 28.08.2020
         * 
         * - SerialPortDetect metodu guncellenmistir.
         * - Form isminde degisiklik yapilmistir.
         */

        /* Versiyon: 1.0.12
         * Tarih: 28.08.2020
         * 
         * - Full Erase Chip komutu altında göndermeden once deviceMemory.ClearAll() silme komutu eklendi.
         * - Form'a icon eklenmistir.
         * - SerialPortDetect metodundaki hatalar duzeltilmistir.
         */

        /* Versiyon: 1.0.11
         * Tarih: 28.08.2020
         * 
         * - DataChunk ve Device nesnelerinde duzenlemeler yapilmistir.
         * - HexFile nesnesi eklenip DataChunk classindan ayristirilmistir.
         * - Hex dosyasini stm32 cihazina gondermek icin VeriPaketOlustur metodu ve VerifyClick eventi eklenmistir.
         */

        /* Versiyon: 1.0.10
         * Tarih: 28.08.2020
         * 
         * - Stm32 cihazi icin Device nesnesi eklenmistir.
         * - Cihaza ozel olan flashSize, uniqueID ve diger ozellikler Device nesnesi icinde saklanmistir.
         * - Otomatik olarak cihazın com portunu bulmasi icin SerialPortDetect metodu eklenmistir.
         */

        /* Versiyon: 1.0.9
         * Tarih: 27.08.2020
         * 
         * - 8/16/32 bit gösterimdeki hata duzeltildi.
         */

        /* Versiyon: 1.0.8
         * Tarih: 27.08.2020
         * 
         * - SerialPortLib.dll degisiklik yapilmistir.
         */

        /* Versiyon: 1.0.7
         * Tarih: 27.08.2020
         * 
         * - FlashSizeTopla() ve UniqueIDTopla() metodlarında düzeltmeler yapilmistir.
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

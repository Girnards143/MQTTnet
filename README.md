# MQTTnet
### TR
  Hedy markalı yoğun bakım ünitesi olan pompa cihazının MQTT protokolü ile gerçekleştirdiği veri haberleşmesi için yazdığım subscriber
  client kodudur. Kod kısaca cihazın serverda belli bir topice yaptığı yayını dinleyip gelen json formatındaki datayı convert ederek
  *data.txt* adlı metin dosyasına daha anlamlı bir biçimde yazıyor.
     
  --MQTTnet kütüphanesi kullanılarak yapılmıştır.
  
  --Cihaz ve MQTT Broker aynı yerel ağda bulunmaktadır.

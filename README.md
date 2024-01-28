# Vaka Çalışması Web Api

## Çalıştırma
Database klasöründeki dosyadaki script çalıştırılarak veri tabanı kurulmalıdır.
Ardından `dotnet watch run` komutuyla uygulama başlatıldıktan sonra Swagger UI ile API'ye requestler göndermek mümkündür.

## Veri Tabanı
Veri tabanında Employees, Attendences ve PaymentTypes olmak üzere 3 tablo bulunmaktadır. 
* Employees tablosunda çalışanların TCKNO, ad, soyad bilgilerinin yanında hangi tip çalışan oldukları tutulmaktadır.
* Attendences tablosunda çalışanların her yılın her ayı için işe geldikleri gün sayısı ve yaptıkları fazladan mesai saatleri tutulmaktadır.
* PaymentTypes tablosunda ise sabit, günlük ve saatlik ücret bilgileri tutulmaktadır. Bu örnek sorunun karmaşıklığının artmaması için her çalışana ayrı ücret atamak yerine bu şekilde merkezi bir tablo kullanılmıştır.

## Unit Testler
Projenin unit testleri NUnit kullanılarak yazılmıştır. Uygulamanın tek proje olması istendiğinden unit test dosyaları Tests adlı klasör içinde bulunmaktadır.

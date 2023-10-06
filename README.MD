# Options pattern in ASP.NET Core
Options Pattern, uygulama ayarlarını yapılandırmak ve yönetmek için kullanılır. Uygulamanın **farklı bileşenlerinin** veya **hizmetlerinin** belirli ayarları almasına ve bu ayarların merkezi bir konfigürasyon kaynağından okunmasına olanak tanır. Options Pattern, uygulama ayarlarının daha kolay yönetilmesini, değiştirmesini ve test edilmesini sağlar.

Ayrıca Options Pattern uygulama ayarlarını **doğrulamak** için bir mekanizma sağlar.

## By using Configuration.Bind or Configuration.Get<T>
Her iki extension metot sayesinde JSON yapılandırma dosyasından (appsettings.json) uygulama ayarları okunabilir. Ayrıca uygulama ayağa kaldırıldıktan sonra appsettings.json dosyasında güncellenen ayarlarda okunabilir.

## By IOptions<T> interface
Bu arayüz sayesinde appsettings.json dosyasından uygulama ayarları okunur ve **dependency injection service container'a** ayarların bağlandığı sınıf kaydedilir. Bu sayede daha sonra uygulamanın herhangi bir bileşeninde DI' dan talep edilip kullanılabilir.

Uygulama ayağa kaldırıldıktan sonra appsettings.json dosyasında güncellenen uygulama ayarları IOptions<T> arayüzü ile okunamaz. Güncellenen uygulama ayarlarının okunabilmesi için **IOptionsSnapshot** veya **IOptionsMonitor** arayüzleri kullanılır.

### Read updated configuration settings
Uygulama ayağa kalktıktan sonra appsettings.json dosyasından güncellenen bir ayarı okumak için **IOptionsSnapshot** veya **IOptionsMonitor** arayüzlerinden herhangi biri senaryoya göre kullanılabilir.

**What are different between IOptionsSnapshot and IOptionsMonitor:**

* IOptionsMonitor, bir **Singleton** servisdir. Herhangi bir zamanda JSON yapılandırma dosyasındaki güncel verileri anlık okuyabilir. Buda real-time verilere ihtiyaç duyuluğunda kullanılması gereken interface olduğu anlamına gelir. 
    - Özellikle **Singleton** yaşam döngüsüne sahip nesneler için tasarlanmıştır. 
* IOptionsSnapshot, bir **Scoped** servisdir. IOptionsSnapshot<T> nesnesi IoC container dan istendiğinde JSON yapılandırma dosyasındaki verinin anlık görüntüsünü alır ve nesne yaşam döngüsü boyunca bu veriler ile çalışır. Bu yüzden bu arayüz her request' te JSON yapılandırma dosyasındaki verilerin tekrar okunmasını gerektiren senaryolarda kullanılması yararlıdır. Aksi bir durum söz konusu ise bu uygulamanın maliyetini arttırır. Çünkü her request oluştuğunda .json uzantılı bir dosya okunması gerekir. 
    - **Transient** ve **Scoped** yaşam döngüsüne sahip nesneler için tasarlanmıştır.
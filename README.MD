# Options pattern in ASP.NET Core
Options Pattern, uygulama ayarlarını yapılandırmak ve yönetmek için kullanılır. Uygulamanın **farklı bileşenlerinin** veya **hizmetlerinin** belirli ayarları almasına ve bu ayarların merkezi bir konfigürasyon kaynağından okunmasına olanak tanır. Options Pattern, uygulama ayarlarının daha kolay yönetilmesini, değiştirmesini ve test edilmesini sağlar.

Ayrıca Options Pattern uygulama ayarlarını **doğrulamak** için bir mekanizma sağlar.

## By using Configuration.Bind or Configuration.Get<T>
Her iki extension metot sayesinde JSON yapılandırma dosyasından (appsettings.json) uygulama ayarları okunabilir. Ayrıca uygulama ayağa kaldırıldıktan sonra appsettings.json dosyasında güncellenen ayarlarda okunabilir.


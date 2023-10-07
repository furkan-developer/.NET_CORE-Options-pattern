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

## Options validation
ASP.NET Core Options pattern, JSON yapılandırma dosyasından okunan veriler için bir doğrulama mekanizması destekler. Bu mekanizmayı kullanabilmek için program.cs de Options pattern yapılandırması ayarlanır iken **ValidationDataAnnotations** fonksiyonun çağrılması yeterli olacaktır. 

**Not:** Options pattern doğrulama hataları için built-in **OptionsValidationException** tipi kullanılabilir.

```csharp
// Program.cs
builder.Services.AddOptions<PersonOptions>()
    .Bind(builder.Configuration.GetSection(PersonOptions.Person))
    // Enables validation
    .ValidateDataAnnotations();
```

```csharp
// PersonOptions.cs
public class PersonOptions
    {
        public const string Person = "Person";

        // Validation rule
        [MaxLength(10,ErrorMessage = "Name's max lenght should be 10)]
        public string Name { get; set; }
        public string SurName { get; set; }
    }
```

```csharp
[HttpGet]
public IActionResult GetPersonOptions()
{
    PersonOptions personOptions;
    try
    {
        // _personOptions is used here for taking PersonOptions object from DI container
        personOptions = _personOptions.Value;
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
    // catch (OptionsValidationException optValEx)
    // {
    //     return BadRequest(optValEx.Message);
    // }

    return Ok(_personOptions.Value);
}
```

### Complex validation rules
**Validate** fonksiyonu ile complex doğrulama kuralları oluşturulabilir.

```csharp 
builder.Services.AddOptions<PersonOptions>()
    .Bind(builder.Configuration.GetSection(PersonOptions.Person))
    .ValidateDataAnnotations()
    .Validate((personOptions) =>
    {
        // do something
        string name = personOptions.Name.ToLower();
        if(name.Equals("furkan"))
            return personOptions.SurName.ToLower() == "aydın" ? true : false;  
        return true;  
    },"Should be equal surname property to 'aydın' when name propery was 'furkan'");
```

**Not:** Program.cs dosyasında oluşturulan doğrulama kodlarını bir sınıf içerisine taşıyabilmek için ilgili sınıfın **IValidateOptions<>** arayüzünden implementasyon alması gerekir.

```csharp
public class PersonOptionsValidation : IValidateOptions<PersonOptions>
{
    private readonly PersonOptions _personOptions;

    public PersonOptionsValidation(IConfiguration configuration)
    {
        _personOptions = configuration.GetSection(PersonOptions.Person).Get<PersonOptions>();
    }

    public ValidateOptionsResult Validate(string name, PersonOptions options)
    {
        string errorMessages = null;
        
        // use _personOptions for validating PersonOptions properties

        if(errorMessages != null)
            return ValidateOptionsResult.Fail(errorMessages);

        return ValidateOptionsResult.Success;
    }
}
```

```csharp
// Program.cs
builder.Services.Configure<PersonOptions>(builder.Configuration.GetSection(PersonOptions.Person));
builder.Services.AddSingleton<IValidateOptions<PersonOptions>, PersonOptionsValidation>();
``` 

## Acces options in Program.cs
Program.cs dosyasında **IOptions<>** veya **IOptionsMonitor<>** arayüzleri kullanılarak JSON yapılandırma dosyasındaki veriler aşağıdaki gibi ulaşılabilir.

```csharp
var name = app.Services.GetRequiredService<IOptionsMonitor<PersonOptions>>().CurrentValue.Name;
```


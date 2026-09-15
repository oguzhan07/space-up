# Space UP

**Akdeniz Game Jam 2026**'da 3 kişilik ekiple 48 saatte yapılan 3B oyun.

Büyük bir yatak odasında küçük bir karakteri yönetiyorsun. Odayı geziyor, nesnelerle etkileşime
giriyor ve enerjin (sanity) tükenmeden hayatta kalmaya çalışıyorsun.

<!-- Buraya oynanış GIF'i ve ekran görüntüleri ekle:
![Oynanış](docs/gameplay.gif)
-->

## Oyunun çekirdek mekaniği: enerji ve biçim değiştirme

Oyunun tamamı tek bir gerilim üzerine kurulu: **enerjin sürekli azalıyor ve onu durduramıyorsun.**

İki karakter biçimi arasında geçiş yapabiliyorsun:

| Biçim | Özellik | Enerji tüketimi |
|---|---|---|
| **Mini** | Küçük, dar yerlere girer | Yavaş |
| **Büyük** | Güçlü, yüksek yerlere erişir | Hızlı |

**F** tuşuyla geçiş yapılıyor ama geçişin kendisi de anlık enerji harcıyor ve bir bekleme süresi
var. Yani "büyük biçimde kal, güçlü ol" diye bir strateji mümkün değil — oyuncu sürekli
"şimdi hangi biçime ihtiyacım var?" diye karar vermek zorunda. Tasarımın amacı buydu.

Enerji belirli bir eşiğin altına düştüğünde **ekran yavaş yavaş kararmaya başlıyor**. Bu sadece
görsel bir efekt değil, oyuncuya kalan süreyi arayüze bakmadan hissettiren bir uyarı. Enerji
sıfırlanınca oyun bitiyor ve Game Over sahnesi yükleniyor.

Kararmanın başlangıç eşiği, en koyu değeri, biçim başına tüketim hızı, geçiş maliyeti ve bekleme
süresi — hepsi Inspector'dan ayarlanabilir alanlar olarak dışarı çıkarıldı. Jam sırasında oyunun
dengesini kod değiştirmeden ayarlayabilmek için böyle yapıldı.

## Diğer sistemler

**Menü ve sahne geçişi** — Play'e basıldığında kamera yumuşak bir yay çizerek hareket ediyor,
ekran kararıyor ve oyun sahnesi yükleniyor. Menüden oyuna geçiş kesme değil, tek bir akış.

**Nesne etkileşimi** — Odadaki kitaplara yaklaşınca nesne parlayıp dönmeye başlıyor; **E** ile
sayfa arayüzü açılıyor. Etkileşim mesafesi ve arka arkaya basmayı engelleyen bekleme süresi
ayarlanabilir.

**Ses** — Ayrı müzik ve efekt yöneticileri, oyuncuya ait sesler için ayrı bir yönetici.

## Kod yapısı

Bize ait kod `Assets/a Scripts` ve `Assets/Scripts` klasörlerinde:

| Script | Ne yapar |
|---|---|
| `SanityManager.cs` | Enerji barı, ekran kararması, biçim değiştirme, oyun sonu |
| `CharacterSwitch.cs` | İki karakter arasında geçiş, Cinemachine kamerasının hedefini devretme |
| `CameraArcMovement.cs` | Menüdeki yay hareketi ve fade ile sahne geçişi |
| `MouseLook.cs` | Fareyle etrafa bakma |
| `GroundDetector.cs` | Karakter yerde mi kontrolü |
| `BookInteraction.cs` | Kitap etkileşimi ve sayfa arayüzü |
| `MenuManager.cs` | Menü butonları |
| `MusicManager.cs`, `SoundManager.cs`, `PlayerSoundManager.cs` | Ses yönetimi |

## Kontroller

| Tuş | Hareket |
|---|---|
| W A S D | Hareket |
| Fare | Etrafa bakma |
| F | Biçim değiştir (mini / büyük) |
| E | Etkileşim |

## Kullanılan teknolojiler

Unity 6 (6000.4.3f1) · C# · URP · Cinemachine

## Görseller ve modeller

Projedeki 3B modeller, karakterler ve animasyonlar ücretsiz hazır asset paketlerinden geliyor
(KidsCharacterFree, DavidJalbert LowPolyPeople, ithappy Animals_FREE ve diğerleri) ve kendi
lisansları altında kullanılıyor. **Oyunun kodu ekibimize ait.**

## Jam notu

48 saatte yazıldı. Kod jam temposunu yansıtıyor — klasör isimleri ve bazı yapılar aceleyle
kurulmuş durumda. Oyunun tasarım fikri ve çalışan mekaniği asıl çıktı.

# 3D Top Toplama Oyunu

**Ders:** DOT5105 - Oyun İçin Kod Yazımı (Vize Ödevi)
**Motor / Dil:** Unity 6 (URP) / C#

## Oyunun Açıklaması

Oyuncu, bir küreyi (sphere) yön tuşlarıyla hareket ettirip oyun alanındaki topları toplar. **Yeşil** toplar **+10**, **kırmızı** toplar **-5** puan verir. Oyuncu, **50 puana** ulaştıktan sonra turkuaz renkli küpe (EndCube) girmelidir. Yeterli puanla girildiğinde sahne yeniden yüklenir: puan sıfırlanır, oyuncu başlangıç konumuna döner ve toplar tekrar görünür.

**Kontroller:** WASD / Yön tuşları = hareket, Space = zıplama

## Scriptin Görevi

| Script                | Görevi                                                                                                                                                |
| --------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| `PlayerController.cs` | Oyunun tüm mantığı bu script'te toplandı: oyuncu hareketi, zıplama, zemin kontrolü, nesne toplama, puan hesaplama, bitiş kontrolü ve yeniden başlatma |

Script içindeki metotlar:
- `Start()`: `GetComponent` ile Rigidbody bileşenine ulaşır.
- `Update()`: WASD girdisini okur, Space'e basılınca ve `isGrounded` true ise zıplatır.
- `FixedUpdate()`: `AddForce()` ile oyuncuyu hareket ettirir.
- `OnCollisionEnter()` / `OnCollisionExit()`: Zemine temas edip etmediğini `isGrounded` ile tutar.
- `OnTriggerEnter()`: `CompareTag()` ile nesneyi tanır. `Green` +10, `Red` -5 puan verir ve `Destroy()` ile nesneyi kaldırır. `Finish` içinde puan kontrolü yapar.
- `AddScore()`: Puanı günceller.
- `RestartGame()`: `SceneManager` ile sahneyi yeniden yükler.
- `OnGUI()`: Puanı ve uyarı mesajını ekranda gösterir.

## Sahne Düzeni

- **Player:** Sphere, Rigidbody ve `PlayerController` (moveSpeed 6, jumpForce 7)
- **Floor:** Cube, tag: `Ground`
- **Walls:** Oyun alanının dış duvarları
- **RoomWalls:** İç bölmenin duvarları, içeriye yalnızca kapıdan girilir
- **Crates ve Pillars:** Engeller, üzerine çıkılabilenlere `Ground` tag'i verildi
- **GreenBalls (8):** Is Trigger açık, tag: `Green`
- **RedBalls (3):** Is Trigger açık, tag: `Red`
- **EndCube:** Camgöbeği bitiş küpü, Is Trigger açık, tag: `Finish`. Alçak engellerle çevrilidir, oyuncu zıplayarak ulaşmalıdır
- **CinemachineCamera:** Follow modunda, hedef: Player

## Algoritma: isGrounded ile Zıplama

1. `OnCollisionEnter()` içinde, çarpılan nesnenin tag'i `Ground` ise `isGrounded = true` yapılır.
2. `OnCollisionExit()` içinde, zeminden ayrılınca `isGrounded = false` yapılır.
3. `Update()` içinde Space'e basıldığında **ve** `isGrounded` true ise `rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse)` uygulanır.
4. Oyuncu havadayken `isGrounded` false olduğu için ikinci zıplama engellenir.

## Hata Analizi

**Hata:** Oyun çalışınca `InvalidOperationException: You are trying to read Input using the UnityEngine.Input class, but you have switched active Input handling to Input System package` hatası alındı ve oyuncu hareket etmedi.

**Sebep:** Proje yeni Input System paketi ile oluşturulmuştu, ancak kodda eski `Input.GetAxis()` ve `Input.GetKeyDown()` metotları kullanılıyordu.

**Çözüm:** Project Settings -> Player -> Other Settings -> Active Input Handling ayarı **Both** yapıldı. Unity yeniden başlatılınca hata kayboldu ve hareket çalıştı.

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(AudioSource))]
public class VRGunShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;
    public float bulletLife = 3f;
    public float recoilForce = 0.1f;
    
    [Header("Sound Effects")]
    public AudioClip shootSound;
    
    private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;
    
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        
        // Set up trigger press event
        grabInteractable.activated.AddListener(ShootBullet);
    }
    
    private void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.activated.RemoveListener(ShootBullet);
    }
    
    private void ShootBullet(ActivateEventArgs arg)
    {
        // Play shoot sound
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        
        // Spawn bullet
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }
            
            // Destroy bullet after some time
            Destroy(bullet, bulletLife);
            
            // Apply recoil
            ApplyRecoil();
        }
    }
    
    private void ApplyRecoil()
    {
        Rigidbody gunRigidbody = GetComponent<Rigidbody>();
        if (gunRigidbody != null)
        {
            gunRigidbody.AddForce(-bulletSpawnPoint.forward * recoilForce, ForceMode.Impulse);
        }
    }
}
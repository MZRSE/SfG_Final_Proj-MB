using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaintballGun : MonoBehaviour
{
    [SerializeField] private InputActionReference _launcherOnShoot;

    [SerializeField] private LayerMask _layersToShoot = -1;
    [SerializeField] private float _shootDistance = 30;
    [SerializeField] private Camera _camera;
    [SerializeField] private ParticleSystem _impactParticle;
    [SerializeField] private AudioClip _spraySound;

    private void OnEnable()
    {
        _launcherOnShoot.action.performed += PaintballShoot;
        _launcherOnShoot.action.Enable();
    }

    private void OnDisable()
    {
        _launcherOnShoot.action.performed -= PaintballShoot;
        _launcherOnShoot.action.Disable();
    }

    public void PaintballShoot(InputAction.CallbackContext txt)
    {
        Vector3 rayStartPos = _camera.transform.position;
        Vector3 rayDirection = _camera.transform.forward;
        Debug.DrawRay(rayStartPos, rayDirection * _shootDistance, Color.cyan, 1);
        RaycastHit hitInfo;
        if (Physics.Raycast(rayStartPos, rayDirection, out hitInfo, _shootDistance, _layersToShoot))
        {
            if (_impactParticle != null)
            {
                Instantiate(_impactParticle, hitInfo.point, Quaternion.identity);
            }
            if (_spraySound != null)
            {
                SoundPlayer.PlayClip3d(_spraySound, (float)0.5, hitInfo.point);
            }
            Shootable shootableObject = hitInfo.transform.GetComponent<Shootable>();
            if (shootableObject != null)
            {
                shootableObject.TargetShoot();
            }
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace Digger.Tools
{
    public class ShovelSettings : MonoBehaviour
    {
        [Header("Shovel animation")]
        [SerializeField] private Transform shovel;
        [SerializeField] private float swingDuration = 0.2f;
        [SerializeField] private float swingAngle = 60f;

        private Vector3 originalRotation;

        private void Awake()
        {
            originalRotation = shovel.localEulerAngles;
        }

        public void Swing()
        {
            shovel.DOKill();

            Vector3 swingForward = originalRotation + new Vector3(-swingAngle, 0f, 0f);

            shovel
                .DOLocalRotate(swingForward, swingDuration * 0.5f, RotateMode.Fast)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    shovel
                        .DOLocalRotate(originalRotation, swingDuration * 0.5f, RotateMode.Fast)
                        .SetEase(Ease.InQuad);
                });
        }
    }
}
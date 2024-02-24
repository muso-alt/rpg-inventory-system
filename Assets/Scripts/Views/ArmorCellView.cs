using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Views
{
    public class ArmorCellView : MonoBehaviour
    {
        public ItemView ChildItem { get; set; }
        
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_Text _armorValueText;
        [SerializeField] private Image _damageAnimation;

        private Tween _animationTween;

        public RectTransform Rect => _rect;

        public void SetArmor(int armorValue)
        {
            _armorValueText.text = armorValue.ToString();
        }

        public void SetAnimation()
        {
            AnimateDamageGetAsync().Forget();
        }

        private async UniTask AnimateDamageGetAsync()
        {
            if (_animationTween != null && _animationTween.IsPlaying())
            {
                return;
            }
            
            _animationTween = _damageAnimation.DOFade(1f, .2f).SetEase(Ease.InBounce);

            await _animationTween.AsyncWaitForCompletion();
            
            _animationTween = _damageAnimation.DOFade(0, .2f).SetEase(Ease.InBounce);
            
            await _animationTween.AsyncWaitForCompletion();

            _animationTween = null;
        }
    }
}
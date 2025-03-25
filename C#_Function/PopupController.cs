
팝업 패널에 대한 애니메이션과 동작에 대해 작성을 했습니다.
https://velog.io/@jocastle98/%EB%A9%8B%EC%9F%81%EC%9D%B4%EC%82%AC%EC%9E%90%EC%B2%98%EB%9F%BC-%EB%B6%80%ED%8A%B8%EC%BA%A0%ED%94%84-TIL-%ED%9A%8C%EA%B3%A0-%EC%9C%A0%EB%8B%88%ED%8B%B0-%EA%B2%8C%EC%9E%84-%EA%B0%9C%EB%B0%9C-3%EA%B8%B0-59%EC%9D%BC%EC%B0%A8-%ED%8C%9D%EC%97%85-%EB%A7%8C%EB%93%A4%EA%B8%B0
자세한 내용은 위 블로그 참조
아래 MainPanelController는 버튼이 있는 경우 PopupPanel에 대해 생성하는 코드이고
생성이 되었다면 PopupPanelController에 있는 동작에 대해 실행하게 됩니다.


바로 아래것은 MonoBehavior대신 PopupController로 진행하면 좋을거같같아
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class PopupPanelController : MonoBehaviour
{
    [SerializeField] private RectTransform panelRectTransform;  // 팝업될 Panel 자신의 RectTransform
    
    private CanvasGroup mBackgroundCanvasGroup;                 // Panel 뒤를 가릴 (검은)배경
    
    private void Awake()
    {
        mBackgroundCanvasGroup = GetComponent<CanvasGroup>();
    }
    
    /// <summary>
    /// Panel을 나타나게하는 메서드
    /// </summary>
    public virtual void Show()
    {
        // 보여지기 전 초기화
        mBackgroundCanvasGroup.alpha = 0;
        panelRectTransform.localScale = Vector3.zero;
        
        // 배경은 등속으로 등장, 패널은 튕기듯이 등장
        mBackgroundCanvasGroup.DOFade(1, 0.3f).SetEase(Ease.Linear);
        panelRectTransform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Panel을 사라지게하는 메서드
    /// </summary>
    /// <param name="OnPanelControllerHide"></param>
    public virtual void Hide(Action OnPanelControllerHide = null)
    {
        // 사라지기 전 초기화
        mBackgroundCanvasGroup.alpha = 1;
        panelRectTransform.localScale = Vector3.one;
        
        // 배경은 등속으로 퇴장, 패널은 빠르게 작아지다가 끝에서 살짝 당겨지듯 퇴장
        mBackgroundCanvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear);
        panelRectTransform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            // 콜백이 있다면 실행 후 오브젝트 파괴
            OnPanelControllerHide?.Invoke();
            Destroy(gameObject);
        });
    }
}
    
--------------
public class MainPanelController : MonoBehaviour
{
    
    [SerializeField] private GameObject shopPopupPanelPrefab;
 
    #region Main Menu 버튼 클릭 함수
    /// <summary>
    /// Shop 아이콘 터치시 호출되는 메서드
    /// </summary>
    public void OnClickShopButton()
    {
        Instantiate(shopPopupPanelPrefab, canvasTransform);
    }

   
    #endregion
}

public class PopupPanelController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;            // 화면 상단의 타이틀 텍스트
    [SerializeField] private GameObject panelObject;        // 팝업창 오브젝트

    private Image _backgroundImage;
    
    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        
        var color = _backgroundImage.color;
        color.a = 0;
        _backgroundImage.color = color;
        
        panelObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void Start()
    {
        ShowPopupPanel();
    }

    /// <summary>
    /// 타이틀 텍스트에 타이틀 지정 함수
    /// </summary>
    /// <param name="title">타이틀</param>
    public void SetTitleText(string title)
    {
        titleText.text = title;
    }

    /// <summary>
    /// 닫기 버튼을 클릭했을때 실행되는 함수
    /// </summary>
    public void OnClickCloseButton()
    {
        HidePopupPanel();
    }

    private void ShowPopupPanel()
    {
        _backgroundImage.DOFade(0, 0);
        panelObject.GetComponent<CanvasGroup>().DOFade(0, 0);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(-500f, 0);
        
        _backgroundImage.DOFade(1f, 0.2f);
        panelObject.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.2f);
    }

    private void HidePopupPanel()
    {
        _backgroundImage.DOFade(1, 0);
        panelObject.GetComponent<CanvasGroup>().DOFade(1, 0);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(0f, 0);
        
        _backgroundImage.DOFade(0f, 0.2f);
        panelObject.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
        panelObject.GetComponent<RectTransform>().DOAnchorPosY(-500f, 0.2f)
            .OnComplete(() => Destroy(gameObject));
    }
}



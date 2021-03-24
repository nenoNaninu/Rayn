/**
 * A sample script of UniWindowContoller
 * 
 * Author: Kirurobo http://twitter.com/kirurobo
 * License: MIT
 */

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kirurobo
{
    /// <summary>
    /// WindowControllerの設定をToggleでオン／オフするサンプル
    /// </summary>
    public class UiSampleController : MonoBehaviour
    {
        private UniWindowController uniwinc;
        private UniWindowMoveHandle uniWinMoveHandle;
        private RectTransform canvasRect;

        private float mouseMoveSS = 0f;           // Sum of mouse trajectory squares. [px^2]
        private float mouseMoveSSThreshold = 36f; // Click (not dragging) threshold. [px^2]
        private Vector3 lastMousePosition;        // Right clicked position.
        private float touchDuration = 0f;
        private float touchDurationThreshold = 0.5f;   // Long tap time threshold. [s]
        private float lastEventOccurredTime = -5f;     // Timestamp the last event occurred [s]
        private float eventMessageTimeout = 5f;        // Show event message while this period [s]

        public Toggle transparentToggle;
        public Toggle topmostToggle;
        public Toggle bottommostToggle;
        [FormerlySerializedAs("maximizedToggle")] public Toggle zoomedToggle;
        public Toggle dragMoveToggle;
        public Toggle allowDropToggle;
        public Dropdown fitWindowDropdown;
        public Button widthDownButton;
        public Button widthUpButton;
        public Button heightDownButton;
        public Button heightUpButton;
        public Dropdown transparentTypeDropdown;
        public Dropdown hitTestTypeDropdown;
        public Toggle clickThroughToggle;
        public Image pickedColorImage;
        public Text pickedColorText;
        public Text messageText;
        public Button menuCloseButton;
        public RectTransform menuPanel;

        /// <summary>
        /// 初期化
        /// </summary>
        void Start()
        {
            // UniWindowController を探す
            uniwinc = GameObject.FindObjectOfType<UniWindowController>();
            
            // UniWindowDragMove を探す
            uniWinMoveHandle = GameObject.FindObjectOfType<UniWindowMoveHandle>();

            // CanvasのRectTransform取得
            if (menuPanel) canvasRect = menuPanel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            // 有効なモニタ数に合わせて選択肢を作成
            UpdateMonitorDropdown();
            
            // Toggleのチェック状態を、現在の状態に合わせる
            UpdateUI();

            if (uniwinc)
            {
                // UIを操作された際にはウィンドウに反映されるようにする
                transparentToggle?.onValueChanged.AddListener(val => uniwinc.isTransparent = val);
                topmostToggle?.onValueChanged.AddListener(val => uniwinc.isTopmost = val);
                bottommostToggle?.onValueChanged.AddListener(val => uniwinc.isBottommost = val);
                zoomedToggle?.onValueChanged.AddListener(val => uniwinc.isZoomed = val);
                allowDropToggle?.onValueChanged.AddListener(val => uniwinc.allowDropFiles = val);

                fitWindowDropdown?.onValueChanged.AddListener(val => SetFitToMonitor(val));

                widthDownButton?.onClick.AddListener(() => uniwinc.windowSize += new Vector2(-100, 0));
                widthUpButton?.onClick.AddListener(() => uniwinc.windowSize += new Vector2(+100, 0));
                heightDownButton?.onClick.AddListener(() => uniwinc.windowSize += new Vector2(0, -100));
                heightUpButton?.onClick.AddListener(() => uniwinc.windowSize += new Vector2(0, +100));
                
                clickThroughToggle?.onValueChanged.AddListener(val => uniwinc.isClickThrough = val);

                transparentTypeDropdown?.onValueChanged.AddListener(val => uniwinc.SetTransparentType((UniWindowController.TransparentType)val));
                hitTestTypeDropdown?.onValueChanged.AddListener(val => uniwinc.hitTestType = (UniWindowController.HitTestType)val);
                menuCloseButton?.onClick.AddListener(CloseMenu);

                if (uniWinMoveHandle) dragMoveToggle?.onValueChanged.AddListener(val => uniWinMoveHandle.enabled = val);

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                // Windows でなければ、透過方法の選択は無効とする
                //if (transparentTypeDropdown) transparentTypeDropdown.interactable = false;
                //if (transparentTypeDropdown) transparentTypeDropdown.enabled = false;
                if (transparentTypeDropdown) transparentTypeDropdown.gameObject.SetActive(false);
#endif
                
                // Add events
                uniwinc.OnStateChanged += () =>
                {
                    Debug.Log("Style changed");
                    UpdateUI();
                };
                uniwinc.OnMonitorChanged += () => {
                    UpdateMonitorDropdown();
                    UpdateUI();
                    ShowEventMessage("Resolution changed!"); 
                };
                uniwinc.OnDropFiles += files =>
                {
                    ShowEventMessage(string.Join(Environment.NewLine, files));
                };
            }
        }

        /// <summary>
        /// Show the message with timeout
        /// </summary>
        /// <param name="message"></param>
        private void ShowEventMessage(string message)
        {
            lastEventOccurredTime = Time.time;
            if (messageText) messageText.text = message;

            Debug.Log(message);
        }

        /// <summary>
        /// 毎フレーム行う処理
        /// </summary>
        private void Update()
        {
            // ヒットテスト関連の表示を更新
            UpdateHitTestUI();

            // 動作確認のためウィンドウ位置・サイズを表示
            if ((lastEventOccurredTime + eventMessageTimeout) < Time.time)
            {
                ShowWindowMetrics();
            }

            // マウス右ボタンクリックでメニューを表示させる。閾値以下の移動ならクリックとみなす。
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePosition = Input.mousePosition;
                touchDuration = 0f;
            }
            if (Input.GetMouseButton(1))
            {
                mouseMoveSS += (Input.mousePosition - lastMousePosition).sqrMagnitude;
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (mouseMoveSS < mouseMoveSSThreshold)
                {
                    ShowMenu(lastMousePosition);
                }
                mouseMoveSS = 0f;
                touchDuration = 0f;
            }
            
            // ロングタッチでもメニューを表示させる
            if (Input.touchSupported && (Input.touchCount > 0))
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastMousePosition = Input.mousePosition;
                    touchDuration = 0f;
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    mouseMoveSS += touch.deltaPosition.sqrMagnitude;
                    touchDuration += touch.deltaTime;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if ((mouseMoveSS < mouseMoveSSThreshold) && (touchDuration >= touchDurationThreshold))
                    {
                        ShowMenu(lastMousePosition);
                    }
                    mouseMoveSS = 0f;
                    touchDuration = 0f;
                }
            }

            // サンプルとしての処理
            if (uniwinc)
            {
                // [Space]キーを押すと強制的にクリックスルーを解除
                // 操作不能となったときの対応
                // ただし自動判定が有効ならすぐ変化の可能性もある
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    uniwinc.isClickThrough = false;
                }
            }

            // Quit or stop playing when pressed [ESC]
            if (Input.GetKey(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        /// <summary>
        /// フィット対象モニタドロップダウンが変更された時の処理
        /// </summary>
        /// <param name="val"></param>
        void SetFitToMonitor(int val)
        {
            if (!uniwinc) return;

            if (val < 1)
            {
                // ドロップダウンの先頭は、フィット無し
                uniwinc.shouldFitMonitor = false;
                
                // 最大化を変更できるようにする
                if (zoomedToggle) zoomedToggle.interactable = true;
            }
            else
            {
                // 次からなので、モニタ番号は1を引く
                uniwinc.monitorToFit = val - 1;
                uniwinc.shouldFitMonitor = true;    // これがfalseからtrueにしたタイミングで移動されるため、モニタ番号を指定してから変更
                
                // 最大化を変更不可にする
                if (zoomedToggle) zoomedToggle.interactable = false;
            }
        }

        /// <summary>
        /// ウィンドウ位置と座標を表示
        /// </summary>
        void ShowWindowMetrics()
        {
            if (uniwinc)
            {
                var winPos = uniwinc.windowPosition;
                OutputMessage(
                    "Pos.: " + winPos
                    + "\nSize: " + uniwinc.windowSize
                    + "\nRel. Cur.:" + (uniwinc.cursorPosition - winPos)
                    + "\nUnity Cur.:" + (Vector2)Input.mousePosition
                    );
            }
        }

        /// <summary>
        /// Refresh UI on focused
        /// </summary>
        /// <param name="hasFocus"></param>
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                UpdateUI();

                if (uniwinc)
                {
                    OutputMessage("Focused");
                }
                else
                {
                    OutputMessage("No UniWindowController");
                }
                
            }
        }

        /// <summary>
        /// 指定した座標にコンテキストメニューを表示する
        /// </summary>
        /// <param name="position">中心座標指定</param>
        private void ShowMenu(Vector2 position)
        {
            if (menuPanel)
            {
                Vector2 pos = position * (canvasRect.sizeDelta.x / Screen.width);
                float w = menuPanel.rect.width;
                float h = menuPanel.rect.height;

                // 指定座標に中心が来る前提で位置調整
                pos.y = Mathf.Max(Mathf.Min(pos.y, Screen.height - h / 2f), h / 2f);   // はみ出していれば上に寄せる
                pos.x = Mathf.Max(Mathf.Min(pos.x, Screen.width - w / 2f), w / 2f);    // 右にはみ出していれば左に寄せる

                menuPanel.pivot = Vector2.one * 0.5f;    // Set the center
                menuPanel.anchorMin = Vector2.zero;
                menuPanel.anchorMax = Vector2.zero;
                menuPanel.anchoredPosition = pos;
                menuPanel.gameObject.SetActive(true);
            }
        }
        
        /// <summary>
        /// コンテキストメニューを閉じる
        /// </summary>
        private void CloseMenu()
        {
            if (menuPanel)
            {
                menuPanel.gameObject.SetActive(false);
            }
        } 

        /// <summary>
        /// 実際の状態をUI表示に反映
        /// </summary>
        private void UpdateUI()
        {
            if (uniwinc)
            {
                if (transparentToggle)
                {
                    transparentToggle.isOn = uniwinc.isTransparent;
                }

                if (topmostToggle)
                {
                    topmostToggle.isOn = uniwinc.isTopmost;
                }
                
                if (bottommostToggle)
                {
                    bottommostToggle.isOn = uniwinc.isBottommost;
                }
                
                if (zoomedToggle)
                {
                    zoomedToggle.isOn = uniwinc.isZoomed;
                }

                if (allowDropToggle)
                {
                    allowDropToggle.isOn = uniwinc.allowDropFiles;
                }

                if (dragMoveToggle)
                {
                    dragMoveToggle.isOn = (uniWinMoveHandle && uniWinMoveHandle.isActiveAndEnabled);
                }

                if (fitWindowDropdown)
                {
                    if (uniwinc.shouldFitMonitor)
                    {
                        fitWindowDropdown.value = uniwinc.monitorToFit + 1;
                        if (zoomedToggle) zoomedToggle.interactable = false;
                    }
                    else
                    {
                        fitWindowDropdown.value = 0;
                        if (zoomedToggle) zoomedToggle.interactable = true;
                    }
                    fitWindowDropdown.RefreshShownValue();
                }

                if (transparentTypeDropdown)
                {
                    transparentTypeDropdown.value = (int)uniwinc.transparentType;
                    transparentTypeDropdown.RefreshShownValue();
                }


                if (hitTestTypeDropdown)
                {
                    hitTestTypeDropdown.value = (int)uniwinc.hitTestType;
                    hitTestTypeDropdown.RefreshShownValue();
                }
                
                // ヒットテスト部分の表示も更新
                UpdateHitTestUI();
            }
        }

        /// <summary>
        /// ヒットテスト関連のUI更新
        /// 自動で変化するため UpdateUI() よりも高頻度で更新の必要がある
        /// </summary>
        public void UpdateHitTestUI()
        {
            if (uniwinc)
            {
                if (clickThroughToggle)
                {
                    clickThroughToggle.isOn = uniwinc.isClickThrough;
                    if (uniwinc.hitTestType == UniWindowController.HitTestType.None)
                    {
                        clickThroughToggle.interactable = true;
                    }
                    else
                    {
                        clickThroughToggle.interactable = false;
                    }
                }

                if (uniwinc.hitTestType == UniWindowController.HitTestType.Opacity && uniwinc.isTransparent)
                {
                    if (pickedColorImage)
                    {
                        pickedColorImage.color = uniwinc.pickedColor;
                    }

                    if (pickedColorText)
                    {
                        pickedColorText.text = $"Alpha:{uniwinc.pickedColor.a:P0}";
                        pickedColorText.color = Color.black;
                    }
                }
                else
                {
                    if (pickedColorImage)
                    {
                        pickedColorImage.color = Color.gray;
                    }

                    if (pickedColorText)
                    {
                        pickedColorText.text = $"Color picker is disabled";
                        pickedColorText.color = Color.gray;
                    }
                }
            }
        }

        /// <summary>
        /// モニタ選択ドロップダウンの選択肢を更新
        /// この後にUpdateUI()を呼ぶこと
        /// </summary>
        void UpdateMonitorDropdown()
        {
            if (!fitWindowDropdown) return;

            // 先頭以外の選択肢を削除
            fitWindowDropdown.options.RemoveRange(1, fitWindowDropdown.options.Count - 1);

            if (!uniwinc)
            {
                fitWindowDropdown.value = 0;
            }
            else
            {
                int count = uniwinc.GetMonitorCount();
                for (int i = 0; i < count; i++)
                {
                    fitWindowDropdown.options.Add(new Dropdown.OptionData("Fit to Monitor " + i));
                }
                if (uniwinc.monitorToFit >= count)
                {
                    uniwinc.monitorToFit = count - 1;
                }
            }
        }

        /// <summary>
        /// テキスト枠がUIにあれば、そこにメッセージを出す。無ければコンソールに出力
        /// </summary>
        /// <param name="text"></param>
        public void OutputMessage(string text)
        {
            if (messageText)
            {
                messageText.text = text;
            }
            else
            {
                Debug.Log(text);
            }
        }
    }
}

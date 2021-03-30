﻿/*
 * UniWindowDragMove.cs
 * 
 * Author: Kirurobo http://twitter.com/kirurobo
 * License: MIT
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace  Kirurobo
{
    public class UniWindowMoveHandle : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private UniWindowController _uniwinc;

        /// <summary>
        /// ウィンドウが最大化されているときは移動を無効にするか
        /// </summary>
        [Tooltip("Disable drag-move when the window is zoomed (maximized).")]
        public bool disableOnZoomed = true;

        /// <summary>
        /// ドラッグ中なら true
        /// </summary>
        public bool IsDragging
        {
            get { return _isDragging; }
        }
        private bool _isDragging = false;

        /// <summary>
        /// ドラッグを行なうか否か
        /// </summary>
        private bool IsEnabled
        {
            get { return enabled && (!disableOnZoomed || !IsZoomed); }
        }

        /// <summary>
        /// モニタにフィットさせるか、最大化している
        /// </summary>
        private bool IsZoomed
        {
            get { return (_uniwinc && (_uniwinc.shouldFitMonitor || _uniwinc.isZoomed)); }
        }

        /// <summary>
        /// ドラッグ前には自動ヒットテストが有効だったかを記憶
        /// </summary>
        private bool _isHitTestEnabled;
        
        /// <summary>
        /// ドラッグ開始時のウィンドウ内座標[px]
        /// </summary>
        private Vector2 _dragStartedPosition;
        
        // Start is called before the first frame update
        void Start()
        {
            // シーン中の UniWindowController を取得
            _uniwinc = GameObject.FindObjectOfType<UniWindowController>();
            if (_uniwinc) _isHitTestEnabled = _uniwinc.isHitTestEnabled;

            //// ↓なくても良さそうなので勝手に変更しないようコメントアウト
            //Input.simulateMouseWithTouches = false;
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// ドラッグ開始時の処理
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsEnabled)
            {
                return;
            }
            
            // Macだと挙動を変える
            //  実際はRetinaサポートが有効のときだけだが、
            //  eventData.position の系と、ウィンドウ座標系でスケールが一致しなくなってしまう
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            _dragStartedPosition = _uniwinc.windowPosition - _uniwinc.cursorPosition;
#else
            _dragStartedPosition = eventData.position;
#endif
        
            // _isDragging が false ならこれからドラッグ開始と判断
            if (!_isDragging)
            {
                // ドラッグ中はヒットテストを無効にする
                _isHitTestEnabled = _uniwinc.isHitTestEnabled;
                _uniwinc.isHitTestEnabled = false;
                _uniwinc.isClickThrough = false;
            }
            
            _isDragging = true;
        }

        /// <summary>
        /// ドラッグ終了時の処理
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragging();
        }

        private void EndDragging()
        {
            if (_isDragging)
            {
                _uniwinc.isHitTestEnabled = _isHitTestEnabled; 
            }
            _isDragging = false;
        }
        
        /// <summary>
        /// 最大化時以外なら、マウスドラッグによってウィンドウを移動
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (!_uniwinc || !_isDragging) return;

            // ドラッグでの移動が無効化されていた場合
            if (!IsEnabled)
            {
                EndDragging();
                return;
            }

            if (eventData.button != PointerEventData.InputButton.Left) return;

            // フルスクリーンならウィンドウ移動は行わない
            //  エディタだと true になってしまうようなので、エディタ以外でのみ確認
#if !UNITY_EDITOR
            if (Screen.fullScreen)
            {
                EndDragging();
                return;
            }
#endif

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            // Macの場合、ネイティブプラグインでのカーソル位置取得・設定
            _uniwinc.windowPosition = _uniwinc.cursorPosition + _dragStartedPosition;
#else
            // Windowsなら、タッチ操作も対応させるために eventData.position を使用する
            // スクリーンポジションが開始時の位置と一致させる分だけウィンドウを移動
            _uniwinc.windowPosition += eventData.position - _dragStartedPosition;
#endif
        }
    }
}

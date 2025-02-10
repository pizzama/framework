using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 打字机效果 type text
/// </summary>
namespace SFramework.Components
{
    public abstract class TypeComponent : MonoBehaviour
    {
        public delegate void OnComplete();

        [SerializeField] private float _defaultSpeed = 0.05f;


        private string _currentText;
        protected string finalText;
        private Coroutine _typeTextCoroutine;

        private static readonly string[] _uguiSymbols = { "b", "i", "s", "u" };
        private static readonly string[] _uguiCloseSymbols = { "b", "i", "s", "u", "size", "color", "line-indent" };
        private OnComplete _onCompleteCallback;

        protected abstract void Init();

        public void Awake()
        {
            Init();
        }

        public virtual void Clean()
        {
            _currentText = "";
            finalText = "";
        }

        public virtual void SetText(string text, float speed = -1)
        {
            Init();

            _defaultSpeed = speed > 0 ? speed : _defaultSpeed;
            finalText = ReplaceSpeed(text);
            if (_typeTextCoroutine != null)
            {
                StopCoroutine(_typeTextCoroutine);
            }

            _typeTextCoroutine = StartCoroutine(TypeText(text));
        }

        protected abstract Component getCompoent();

        public void SkipTypeText()
        {
            if (getCompoent() == null || finalText == null)
                return;
            if (_typeTextCoroutine != null)
                StopCoroutine(_typeTextCoroutine);
            _typeTextCoroutine = null;
            skipHandle();
            if (_onCompleteCallback != null)
                _onCompleteCallback();
        }

        protected abstract void skipHandle();

        protected abstract float getSize();

        protected abstract void changeText(string value);

        public IEnumerator TypeText(string text)
        {
            _currentText = "";

            var len = text.Length;
            var speed = _defaultSpeed;
            var tagOpened = false;
            var tagType = "";
            for (var i = 0; i < len; i++)
            {
                if (text[i] == '[' && i + 6 < len && text.Substring(i, 7).Equals("[speed="))
                {
                    var parseSpeed = "";
                    for (var j = i + 7; j < len; j++)
                    {
                        if (text[j] == ']')
                            break;
                        parseSpeed += text[j];
                    }

                    if (!float.TryParse(parseSpeed, out speed))
                        speed = 0.05f;

                    i += 8 + parseSpeed.Length - 1;
                    continue;
                }

                // ngui color tag
                if (text[i] == '[' && i + 7 < len && text[i + 7] == ']')
                {
                    _currentText += text.Substring(i, 8);
                    i += 8 - 1;
                    continue;
                }

                var symbolDetected = false;
                for (var j = 0; j < _uguiSymbols.Length; j++)
                {
                    var symbol = string.Format("<{0}>", _uguiSymbols[j]);
                    if (text[i] == '<' && i + (1 + _uguiSymbols[j].Length) < len &&
                        text.Substring(i, 2 + _uguiSymbols[j].Length).Equals(symbol))
                    {
                        _currentText += symbol;
                        i += (2 + _uguiSymbols[j].Length) - 1;
                        symbolDetected = true;
                        tagOpened = true;
                        tagType = _uguiSymbols[j];
                        break;
                    }
                }

                if (text[i] == '<' && i + (1 + 13) < len && text.Substring(i, 2 + 6).Equals("<color=#") &&
                    text[i + 14] == '>')
                {
                    _currentText += text.Substring(i, 6 + 8 + 1);
                    i += (1 + 14) - 1;
                    symbolDetected = true;
                    tagOpened = true;
                    tagType = "color";
                }

                if (text[i] == '<' && i + 5 < len && text.Substring(i, 6).Equals("<size="))
                {
                    var parseSize = "";
                    var size = getSize();
                    for (var j = i + 6; j < len; j++)
                    {
                        if (text[j] == '>') break;
                        parseSize += text[j];
                    }

                    if (float.TryParse(parseSize, out size))
                    {
                        _currentText += text.Substring(i, 7 + parseSize.Length);
                        i += (7 + parseSize.Length) - 1;
                        symbolDetected = true;
                        tagOpened = true;
                        tagType = "size";
                    }
                }

                if (text[i] == '<' && i + 12 < len && text.Substring(i, 13).Equals("<line-indent="))
                {
                    var parseSize = "";
                    for (var j = i + 13; j < len; j++)
                    {
                        if (text[j] == '>') break;
                        parseSize += text[j];
                    }

                    _currentText += text.Substring(i, 14 + parseSize.Length);
                    i += (14 + parseSize.Length) - 1;
                    symbolDetected = true;
                    tagOpened = true;
                    tagType = "line-indent";
                }

                // exit symbol
                for (var j = 0; j < _uguiCloseSymbols.Length; j++)
                {
                    var symbol = string.Format("</{0}>", _uguiCloseSymbols[j]);
                    if (text[i] == '<' && i + (2 + _uguiCloseSymbols[j].Length) < len &&
                        text.Substring(i, 3 + _uguiCloseSymbols[j].Length).Equals(symbol))
                    {
                        _currentText += symbol;
                        i += (3 + _uguiCloseSymbols[j].Length) - 1;
                        symbolDetected = true;
                        tagOpened = false;
                        break;
                    }
                }

                if (symbolDetected) continue;

                _currentText += text[i];
                changeText(_currentText + (tagOpened ? string.Format("</{0}>", tagType) : ""));
                yield return new WaitForSeconds(speed);
            }

            _typeTextCoroutine = null;

            if (_onCompleteCallback != null)
                _onCompleteCallback();
        }

        private string ReplaceSpeed(string text)
        {
            var result = "";
            var len = text.Length;
            for (var i = 0; i < len; i++)
            {
                if (text[i] == '[' && i + 6 < len && text.Substring(i, 7).Equals("[speed="))
                {
                    var speedLength = 0;
                    for (var j = i + 7; j < len; j++)
                    {
                        if (text[j] == ']')
                            break;
                        speedLength++;
                    }

                    i += 8 + speedLength - 1;
                    continue;
                }

                result += text[i];
            }

            return result;
        }

        public bool IsSkippable()
        {
            return _typeTextCoroutine != null;
        }

        public void SetOnComplete(OnComplete onComplete)
        {
            _onCompleteCallback = onComplete;
        }
    }

    public static class TypeTextComponentUtility
    {
        public static void TypeText(this TMP_Text label, string text, float speed = 0.05f,
            TypeComponent.OnComplete onComplete = null)
        {
            var typeText = label.GetComponent<TypeComponent>();
            if (typeText == null)
            {
                typeText = label.gameObject.AddComponent<TypeComponent>();
            }

            typeText.SetText(text, speed);
            typeText.SetOnComplete(onComplete);
        }

        public static bool IsSkippable(this TMP_Text label)
        {
            var typeText = label.GetComponent<TypeComponent>();
            if (typeText == null)
            {
                typeText = label.gameObject.AddComponent<TypeComponent>();
            }

            return typeText.IsSkippable();
        }

        public static void SkipTypeText(this TMP_Text label)
        {
            var typeText = label.GetComponent<TypeComponent>();
            if (typeText == null)
            {
                typeText = label.gameObject.AddComponent<TypeComponent>();
            }

            typeText.SkipTypeText();
        }
    }
}
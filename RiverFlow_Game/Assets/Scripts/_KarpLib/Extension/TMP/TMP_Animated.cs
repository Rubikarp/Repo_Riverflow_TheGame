using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

namespace TMPro
{
    public class TMP_Animated : TextMeshProUGUI
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float timeBtwWord = 2f;
        [SerializeField] private AudioSource writingSFX = null;

        public UnityEvent onDialogueEnd;
        private IEnumerator readingRoutine;

        /// <summary>
        /// In seconds
        /// </summary>
        public float TimeForCompletingCurrentText { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            TimeForCompletingCurrentText = 0f;
            ReadText();
        }

        [ContextMenu("ReadText")]
        public void ReadText() => ReadText(text);
        public void ReadText(string displayText)
        {
            if (readingRoutine is null) { }
            else StopCoroutine(readingRoutine);

            if (writingSFX != null)
                writingSFX.Play();

            text = displayText;
            maxVisibleCharacters = 0;

            readingRoutine = ReadingText();
            StartCoroutine(readingRoutine);

            IEnumerator ReadingText()
            {
                if (text == string.Empty) yield break;

                int wordCounter = 0;
                //var wordTexts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var wordTexts = text.Split(new char[]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int visibleCounter = 0;
                float scaledTimeBtwCharacters = 1f / speed;
                float scaledTimeBtwWords = timeBtwWord / speed;

                //TimeForCompletingCurrentText += scaledTimeBtwCharacters * text.Length;
                //TimeForCompletingCurrentText += (scaledTimeBtwWords - scaledTimeBtwCharacters) * wordTexts.Length - 1;

                //Ameliorable
                while (wordCounter < wordTexts.Length)
                {
                    while (visibleCounter < wordTexts[wordCounter].Length)
                    {
                        visibleCounter++;
                        TimeForCompletingCurrentText += scaledTimeBtwCharacters;//Time Btw Letters
                    }
                    TimeForCompletingCurrentText += scaledTimeBtwWords;//Time Btw words

                    visibleCounter = 0;
                    wordCounter++;
                }

                wordCounter = 0;
                wordTexts = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                visibleCounter = 0;

                while (wordCounter < wordTexts.Length)
                {
                    while (visibleCounter < wordTexts[wordCounter].Length)
                    {
                        visibleCounter++;
                        maxVisibleCharacters++;

                        yield return new WaitForSecondsRealtime(scaledTimeBtwCharacters);//Time Btw Letters
                        TimeForCompletingCurrentText -= scaledTimeBtwCharacters;
                    }
                    maxVisibleCharacters++;//For The Space
                    yield return new WaitForSecondsRealtime(scaledTimeBtwWords);//Time Btw words
                    TimeForCompletingCurrentText -= scaledTimeBtwWords;

                    visibleCounter = 0;
                    wordCounter++;
                }
                yield return null;

                TimeForCompletingCurrentText = 0;
                readingRoutine = null;

                if (writingSFX != null)
                    writingSFX.Stop();

                onDialogueEnd.Invoke();
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

public class TypeWriterEffectReadyRoom : MonoBehaviour
{
    //변경할 변수
    public float delay;
    public float Skip_delay;
    public int cnt;
    string[] fulltext;
    public int dialog_cnt;
    string currentText;

    //타이핑확인 변수
    public bool text_exit;
    public bool text_full;
    public bool text_cut;

    [SerializeField]
    public SubArrayReadyRoom[] explainArray;

    public TextMeshProUGUI text;

    //시작과 동시에 타이핑시작
    public void StartTyping(int state, int index)
    {
        if (explainArray[state].explainArrayStep[index].onStartEvent != null) explainArray[(int)state].explainArrayStep[index].onStartEvent.Invoke();
        Debug.Log(state + "/" + index + "/" + explainArray[state].explainArrayStep[index].explains.Length + "/" +
            explainArray[state].explainArrayStep[index].explains);
        Get_Typing(explainArray[state].explainArrayStep[index].explains.Length, explainArray[state].explainArrayStep[index].explains, state, index);
    }
    //
    //
    //
    // //텍스트 시작호출
    public void Get_Typing(int _dialog_cnt, string[] _fullText, int state, int index)
    {
        //재사용을 위한 변수초기화
        text_exit = false;
        text_full = false;
        text_cut = false;
        cnt = 0;

        //변수 불러오기
        dialog_cnt = _dialog_cnt;
        fulltext = new string[dialog_cnt];
        fulltext = _fullText;

        //타이핑 코루틴시작
        StartCoroutine(ShowText(fulltext, state, index));
    }

    IEnumerator ShowText(string[] _fullText, int state, int index)
    {
        //모든텍스트 종료
        Debug.Log(cnt + "/" + dialog_cnt);

        if (cnt >= dialog_cnt)
        {
            text_exit = true;
            Debug.Log(state + "/" + index + "/" + "모든텍스트 종료");
            if (explainArray[state].explainArrayStep[index].onFinishEvent != null)
            {
                Debug.Log("onFinishEvent");
                explainArray[state].explainArrayStep[index].onFinishEvent.Invoke();
            }


            StopCoroutine("showText");


        }
        else
        {

            //기존문구clear
            currentText = "";
          //  Speak(_fullText[cnt]);
            //타이핑 시작
            for (int i = 0; i < _fullText[cnt].Length; i++)
            {
                //타이핑중도탈출
                if (text_cut == true)
                {
                    break;
                }
                //단어하나씩출력
                currentText = _fullText[cnt].Substring(0, i + 1);
                text.text = currentText;
                yield return new WaitForSeconds(delay);
            }
            //탈출시 모든 문자출력
            Debug.Log("Typing 종료");
            text.text = _fullText[cnt];

          //  Debug.Log(Speaker.isSpeaking);
        //    yield return StartCoroutine(CheckEndOfSpeak());
         //   Debug.Log(Speaker.isSpeaking);
            yield return new WaitForSeconds(Skip_delay);
            //스킵_지연후 종료
            Debug.Log("Enter 대기");
            text_full = true;
            End_Typing(state, index);
        }
    }
    public string voiceName;
   // public void Speak(string words)
   // {
   //     Debug.Log("tt");
   //     Speaker.Speak(words, null, Speaker.VoiceForName(voiceName));
   // }

  // IEnumerator CheckEndOfSpeak()
  // {
  //     while (true)
  //     {
  //         yield return null;
  //         if (!Speaker.isSpeaking)
  //         {
  //             break;
  //         }
  //     }
  // }

    //다음버튼함수
    public void End_Typing(int state, int index)
    {
        //다음 텍스트 호출
        if (text_full == true)
        {
            cnt++;
            text_full = false;
            text_cut = false;
            StartCoroutine(ShowText(fulltext, state, index));
        }
        //텍스트 타이핑 생략
        else
        {
            text_cut = true;

        }
    }
}

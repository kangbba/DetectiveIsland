using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using Aroka.Anim;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aroka.ArokaUtils {

    public static class ArokaUtils
    {
        public static List<T> LoadScriptableDatasFromFolder<T>(string folderName) where T : UnityEngine.Object
        {
            var dataList = new List<T>();

            // 폴더 내의 모든 ScriptableObject를 가져와 리스트에 추가
            string folderPath = "Assets/Resources/" + folderName;
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            FileInfo[] files = directoryInfo.GetFiles("*.asset");

            foreach (FileInfo file in files)
            {
                string path = folderName + "/" + Path.GetFileNameWithoutExtension(file.Name);
                T data = Resources.Load<T>(path);

                if (data != null)
                {
                    dataList.Add(data);
                }
            }
            return dataList;
        }
        
        public static List<T> LoadResourcesFromFolder<T>(string folderName) where T : UnityEngine.Object
        {
            var dataList = new List<T>();

            // 폴더 내의 모든 리소스를 가져와 리스트에 추가
            UnityEngine.Object[] resources = Resources.LoadAll(folderName, typeof(T));

            foreach (UnityEngine.Object resource in resources)
            {
                // 가져온 리소스를 T 타입으로 캐스팅하여 리스트에 추가
                dataList.Add(resource as T);
            }

            return dataList;
        }
        
        public static int EnumCount(this Type _type)
        {
            return Enum.GetValues(_type).Length;
        }
    }

    public static class UIExtensions
    {
        public static Vector3 MousePosition
        {
            get
            {
                if (Input.touchCount > 0)
                {
                    return Input.touches[0].position;
                }
                else
                {
                    return Input.mousePosition;
                }
            }
        }

        public static bool IsPointerOverUIObject(Vector2 touchPos)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = touchPos;

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }
    }
    public static class TransformExtensions
    {
        public static T[] GetComponentsInChildren<T>(this Component component, bool includeSelf) where T : Component
        {
            if (includeSelf)
            {
                return component.GetComponentsInChildren<T>();
            }
            else
            {
                return component.GetComponentsInChildren<T>().Where(c => c != component).ToArray();
            }
        }
        public static void DestroyAllChildren(this Transform tr)
        {
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(tr.GetChild(i).gameObject);
            }
        }

        public static void DestroyImmediateAllChildren(this Transform tr)
        {
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(tr.GetChild(i).gameObject);
            }
        }

        public static void SetRayCastTargetRecursively(this Transform _parent, bool b)
        {
            if (null == _parent)
            {
                return;
            }
            if (_parent.GetComponent<Image>() != null)
            {
                _parent.GetComponent<Image>().raycastTarget = b;
            }
            foreach (Transform child in _parent)
            {
                if (null == child)
                {
                    continue;
                }
                child.SetRayCastTargetRecursively(b);
            }
        }
    }

    public static class ArokaAnimExtensions
    {
        public static void SetAnims(this ArokaAnim[] uiAnims, bool isOn, float totalTime)
        {
            for(int i = 0; i < uiAnims.Length; i++)
            {   
                uiAnims[i].SetAnim(isOn, totalTime);
            }
        }
    }
    public static class ListExtensions
    {
        public static List<T> GetRandomList<T>(this List<T> preliminaryList, int countToSelect)
        {
            List<T> selectedList = new List<T>();

            T[] preliminaryArr = preliminaryList.ToArray();
            int preliminaryCount = preliminaryArr.Length;

            for (int i = 0; i < preliminaryCount; ++i)
            {
                int ranIdx = Random.Range(i, preliminaryCount);
                T tmp = preliminaryArr[ranIdx];
                preliminaryArr[ranIdx] = preliminaryArr[i];
                preliminaryArr[i] = tmp;
            }

            for (int i = 0; i < countToSelect && i < preliminaryArr.Length; i++)
            {
                selectedList.Add(preliminaryArr[i]);
            }

            return selectedList;
        }

        public static List<T> MixUpList<T>(this List<T> preliminaryList)
        {
            T[] preliminaryArr = preliminaryList.ToArray();
            int preliminaryCount = preliminaryArr.Length;

            for (int i = 0; i < preliminaryCount; ++i)
            {
                int ranIdx = Random.Range(i, preliminaryCount);
                T tmp = preliminaryArr[ranIdx];
                preliminaryArr[ranIdx] = preliminaryArr[i];
                preliminaryArr[i] = tmp;
            }

            return new List<T>(preliminaryArr);
        }
    }
    public static class LayerExtensions
    {
        public static void SetLayerRecursively(this GameObject obj, int newLayer)
        {
            if (obj == null)
                return;

            obj.layer = newLayer;
            foreach (Transform child in obj.transform)
            {
                if (child != null)
                    child.gameObject.SetLayerRecursively(newLayer);
            }
        }

        public static void SetLayerRecursively(this GameObject obj, LayerMask newLayerMask)
        {
            obj.SetLayerRecursively(newLayerMask.ToLayer());
        }

        public static void SetLayer(this GameObject obj, int newLayer)
        {
            obj.layer = newLayer;
        }

        public static void SetLayer(this GameObject obj, LayerMask newLayerMask)
        {
            obj.layer = newLayerMask.ToLayer();
        }


        public static int ToLayer(this LayerMask layerMask)
        {
            int layerNumber = 0;
            int layer = layerMask.value;

            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }

            return layerNumber - 1;
        }
    }

    public static class DateExtensions
    {
        public static DateTime ToDate(this string dateString)
        {
            return DateTime.Parse(dateString);
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy'/'MM'/'dd");
        }
    }

    public static class RandomExtensions
    {
        public static Quaternion RandomQuaternion()
        {
            return Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        }

        public static Vector3 RandomVector()
        {
            return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        }

        public static Vector3 RandomVector01()
        {
            return new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        }

        public static bool RandomBool()
        {
            return RandomPossibility(.5f);
        }

        public static int RandomBoolInt()
        {
            return RandomBool() ? 1 : -1;
        }

        public static bool RandomPossibility(float perone)
        {
            return Random.Range(0f, 1f) < perone;
        }

    }

    public static class RectExtensions
    {
        public static Rect ModifiedX(this Rect rect, float n)
        {
            return new Rect(n, rect.y, rect.width, rect.height);
        }
        public static Rect ModifiedY(this Rect rect, float n)
        {
            return new Rect(rect.x, n, rect.width, rect.height);
        }
        public static Rect ModifiedWidth(this Rect rect, float n)
        {
            return new Rect(rect.x, rect.y, n, rect.height);
        }
        public static Rect ModifiedHeight(this Rect rect, float n)
        {
            return new Rect(rect.x, rect.y, rect.width, n);
        }
    }
    public static class VectorExtensions
    {

        public static Vector3 ModifiedX(this Vector3 v, float n)
        {
            return new Vector3(n, v.y, v.z);
        }
        public static Vector3 ModifiedY(this Vector3 v, float n)
        {
            return new Vector3(v.x, n, v.z);
        }

        public static Vector3 ModifiedZ(this Vector3 v, float n)
        {
            return new Vector3(v.x, v.y, n);
        }

        public static Vector2 ModifiedX(this Vector2 v, float n)
        {
            return new Vector2(n, v.y);
        }

        public static Vector2 ModifiedY(this Vector2 v, float n)
        {
            return new Vector2(v.x, n);
        }

        public static Vector3Int ModifiedX(this Vector3Int v, int n)
        {
            return new Vector3Int(n, v.y, v.z);
        }
        public static Vector3Int ModifiedY(this Vector3Int v, int n)
        {
            return new Vector3Int(v.x, n, v.z);
        }

        public static Vector3Int ModifiedZ(this Vector3Int v, int n)
        {
            return new Vector3Int(v.x, v.y, n);
        }

        public static Vector2Int ModifiedX(this Vector2Int v, int n)
        {
            return new Vector2Int(n, v.y);
        }

        public static Vector2Int ModifiedY(this Vector2Int v, int n)
        {
            return new Vector2Int(v.x, n);
        }



        public static Color ModifiedAlpha(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }

        public static Vector3 ToAvg(this List<Vector3> _vectorList)
        {
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < _vectorList.Count; i++)
            {
                sum += _vectorList[i];
            }
            return sum / _vectorList.Count;
        }
    }

    // 파일 경로를 통해 시나리오를 로드합니다.
  

    // TextAsset을 통해 시나리오를 로드합니다.
   



    public static class NewToneJsonConverterExtension
    {
        public static T ConvertJsonToClass_FromJsonTextAsset<T>(TextAsset jsonTextAsset, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
            {
                jsonSerializerSettings = JsonSerializerSettings_MaxDetail;
            }
            return JsonConvert.DeserializeObject<T>(jsonTextAsset.text, jsonSerializerSettings);
        }

        public static T ConvertJsonToClass_FromPath<T>(string jsonFileFullPath, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
            {
                jsonSerializerSettings = JsonSerializerSettings_MaxDetail;
            }
            if (!File.Exists(jsonFileFullPath))
            {
                Debug.LogError($"File not found: {jsonFileFullPath}");
                return default(T);
            }
            string json = File.ReadAllText(jsonFileFullPath);
            return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
        }

        public static string ConvertClassToJson(object unityObj, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
            {
                jsonSerializerSettings = JsonSerializerSettings_MaxDetail;
            }
            return JsonConvert.SerializeObject(unityObj, jsonSerializerSettings);
        }

        public static JsonSerializerSettings JsonSerializerSettings_MaxDetail
        {//최대한 디테일하게 NewToneJson을 Serializer할때 쓰는 함수
            get
            {
                return new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    Formatting = Formatting.Indented,
                    Converters = new List<JsonConverter> { new Vector2Converter() },
                    StringEscapeHandling = StringEscapeHandling.Default // Ensuring Hangul is not escaped
                };
            }
        }

        public class Vector2Converter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Vector2);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                Vector2 vector = (Vector2)value;
                JObject obj = new JObject
                {
                    ["x"] = vector.x,
                    ["y"] = vector.y
                };
                obj.WriteTo(writer);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject obj = JObject.Load(reader);
                return new Vector2((float)obj["x"], (float)obj["y"]);
            }
        }
    }
}

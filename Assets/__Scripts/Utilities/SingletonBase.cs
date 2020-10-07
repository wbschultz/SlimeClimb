using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// from post about singleton classes on http://www.unitygeek.com/unity_c_singleton/
public class SingletonBase<T> : MonoBehaviour where T : Component
{
    private static T instance;
    // get or create new instance of class
    public static T Instance {
        get{
            if(instance == null) {
                instance = FindObjectOfType<T>();
                if (instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    // set dontDestroyOnload 
    public virtual void Awake(){
        if(instance == null) {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy (gameObject);
        }
    }
}

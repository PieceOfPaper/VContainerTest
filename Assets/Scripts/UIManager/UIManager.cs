using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public enum UIDepthType
{
    None,
}

public abstract class UIBasePresenter : IUIManagerPresenterController
{
    public abstract string PrefabName { get; }
    public abstract UIDepthType UIDepthType { get; }
    
    
    protected UIBaseBehaviour m_Behaviour;
    UIBaseBehaviour IUIManagerPresenterController.Behaviour => m_Behaviour;

    void IUIManagerPresenterController.CreateBehaviour(UIBaseBehaviour behavior)
    {
        if (m_Behaviour != null)
        {
            //TODO - 무슨상황..?
        }

        Debug.Log($"[UIBasePresenter] CreateBehaviour - {GetType()}");
        m_Behaviour = behavior;
        OnCreateBehaviour();
    }
    
    protected virtual void OnCreateBehaviour() { }

    void IUIManagerPresenterController.DestroyBehaviour()
    {
        if (m_Behaviour == null)
            return;

        Debug.Log($"[UIBasePresenter] DestroyBehaviour - {GetType()}");
        GameObject.Destroy(m_Behaviour.gameObject);
        m_Behaviour = null;
        OnDestroyBehaviour();
    }
    
    protected virtual void OnDestroyBehaviour() { }


    void IUIManagerPresenterController.OpenBehaviour()
    {
        if (m_Behaviour == null)
            return;

        Debug.Log($"[UIBasePresenter] OpenBehaviour - {GetType()}");
        m_Behaviour.gameObject.SetActive(true);
        OnOpenBehaviour();
    }
    
    protected virtual void OnOpenBehaviour() { }

    void IUIManagerPresenterController.CloseBehaviour()
    {
        if (m_Behaviour == null)
            return;

        Debug.Log($"[UIBasePresenter] CloseBehaviour - {GetType()}");
        m_Behaviour.gameObject.SetActive(false);
        OnCloseBehaviour();
    }
    
    protected virtual void OnCloseBehaviour() { }
}

public abstract class UIBaseBehaviour : MonoBehaviour, IUIManagerBehaviourController
{
    protected UIBasePresenter m_Presenter;
    
    void IUIManagerBehaviourController.Initialize(UIBasePresenter presenter)
    {
        m_Presenter = presenter;
        OnInitialized();
    }

    protected virtual void OnInitialized() { }
}

public abstract class UIBasePresenterTemplate<BT> : UIBasePresenter where BT : UIBaseBehaviour
{
    protected BT MyBehaviour => (BT)m_Behaviour;
}

public abstract class UIBaseBehaviourTemplate<PT> : UIBaseBehaviour where PT : UIBasePresenter
{
    protected PT MyPresenter => (PT)m_Presenter;
}

public interface IUIManagerPresenterController
{
    UIBaseBehaviour Behaviour { get; }

    void CreateBehaviour(UIBaseBehaviour behavior);
    void DestroyBehaviour();

    void OpenBehaviour();
    void CloseBehaviour();
}

public interface IUIManagerBehaviourController
{
    void Initialize(UIBasePresenter presenter);
}

public class UIManagerFactory
{
    [Inject] protected UIManager uiManager;

    public UIBasePresenter CreatePresenter(IObjectResolver container, System.Type type)
    {
        var instance = System.Activator.CreateInstance(type);
        container.Inject(instance);
        return instance as UIBasePresenter;
    }

    public GameObject Instantiate(IObjectResolver container, UIBasePresenter presenter)
    {
        var prefab = Resources.Load<GameObject>($"Prefab/{presenter.PrefabName}");
        return container.Instantiate(prefab);
    }
}

public class UIManager : MonoBehaviour
{
    private UIManagerLifetimeScope m_LifetimeScope;

    private void Awake()
    {
        m_LifetimeScope = GetComponent<UIManagerLifetimeScope>();
        if (m_LifetimeScope != null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private Dictionary<System.Type, UIBasePresenter> m_Presenters = new Dictionary<System.Type, UIBasePresenter>();

    private void CreatePresenter(System.Type type)
    {
        if (type == null)
            return;

        var factory = m_LifetimeScope.Container.Resolve<System.Func<System.Type, UIBasePresenter>>();
        var instance = factory?.Invoke(type);
        if (instance == null)
            return;

        m_Presenters.Add(type, instance);
    }

    public UIBasePresenter GetPresenter<T>() where T : UIBasePresenter => GetPresenter(typeof(T));

    private UIBasePresenter GetPresenter(System.Type type)
    {
        if (m_Presenters.ContainsKey(type) == false)
        {
            CreatePresenter(type);
            if (m_Presenters.ContainsKey(type) == false)
                return null;
        }

        return m_Presenters[type];
    }


    private void LoadUI(System.Type type)
    {
        if (m_Presenters.ContainsKey(type) == false)
        {
            return;
        }

        if (m_LifetimeScope == null)
        {
            Debug.Log($"[UIManager] LoadUI - Not Found LifetimeScope");
            return;
        }

        var presenter = m_Presenters[type];
        if (presenter is IUIManagerPresenterController controller)
        {
            var factory = m_LifetimeScope.Container.Resolve<System.Func<UIBasePresenter, GameObject>>();
            var instance = factory?.Invoke(presenter);

            var behaviour = instance.GetComponent<UIBaseBehaviour>();
            behaviour.gameObject.SetActive(false);
            controller.CreateBehaviour(behaviour);
        }
    }
    
    public void OpenUI<T>() where T : UIBasePresenter => OpenUI(typeof(T));

    public void OpenUI(System.Type type)
    {
        var presenter = GetPresenter(type);
        if (presenter == null)
        {
            Debug.Log($"[UIManager] OpenUI - Not Exist Presenter ({type})");
            return;
        }

        if (presenter is IUIManagerPresenterController controller)
        {
            if (controller.Behaviour == null)
            {
                LoadUI(type);
                if (controller.Behaviour == null)
                {
                    Debug.Log($"[UIManager] OpenUI - Not Exist Behaviour ({type})");
                    return;
                }
            }

            controller.OpenBehaviour();
        }
    }
    
    public void CloseUI<T>() where T : UIBasePresenter => CloseUI(typeof(T));

    public void CloseUI(System.Type type)
    {
        var presenter = GetPresenter(type);
        if (presenter == null)
        {
            Debug.Log($"[UIManager] CloseUI - Not Exist Presenter ({type})");
            return;
        }
        
        if (presenter is IUIManagerPresenterController controller)
        {
            if (controller.Behaviour == null)
            {
                return;
            }

            controller.CloseBehaviour();
        }
    }
}

//  Modified version of th CSharpNotificationCenter from http://wiki.unity3d.com/index.php?title=CSharpNotificationCenter used under Creative Commons Attribution Share Alike License.

// Original Author: capnbishop

// CSharp Author: thedadymac 

//    NotificationCenter is used for handling messages between GameObjects

//    GameObjects can register to receive specific notifications.  When another objects sends a notification of that type, all GameObjects that registered for it and implement the appropriate message will receive that notification.

//    Observing GameObjetcs must register to receive notifications with the AddObserver function, and pass their selves, and the name of the notification.  Observing GameObjects can also unregister themselves with the RemoveObserver function.  GameObjects must request to receive and remove notification types on a type by type basis.

//    Posting notifications is done by creating a Notification object and passing it to PostNotification.  All receiving GameObjects will accept that Notification object.  The Notification object contains the sender, the notification type name, and an option hashtable containing data.

//    To use NotificationCenter, either create and manage a unique instance of it somewhere, or use the static NotificationCenter.

/*
 * Posting example code:
 * -------------------------
 * NotificationsManager.DefaultNotifier.PostNotification(this, "OnExampleEvent");
 * NotificationsManager.DefaultNotifier.PostNotification(this, "OnExampleEvent", anyObject);
 * NotificationsManager.DefaultNotifier.PostNotification(new NotifactionManager.Notification(this, "OnExampleEvent"));
 * NotificationsManager.DefaultNotifier.PostNotification(new NotifactionManager.Notification(this, "OnExampleEvent", anyObject));
 * 
 * Adding and removing listeners:
 * -------------------------
 * NotificationsManager.DefaultNotifier.AddObserver(this, "OnExampleEvent");
 * NotificationsManager.DefaultNotifier.RemoveObserver(this, "OnExampleEvent");
 * 
 */

// We need a static method for objects to be able to obtain the default notification center.
// This default center is what all objects will use for most notifications.  We can of course create our own separate instances of NotificationCenter, but this is the static one used by all.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationsManager : MonoBehaviour
{
    private static NotificationsManager defaultNotifier;
    public static NotificationsManager DefaultNotifier
    {
        get
        {
            if (!defaultNotifier)
            {
                GameObject notificationObject = new GameObject("Default Notification Center");

                defaultNotifier = notificationObject.AddComponent<NotificationsManager>();
            }

            return defaultNotifier;
        }
    }

    // Our hashtable containing all the notifications.  Each notification in the hash table is an ArrayList that contains all the observers for that notification.
    private Dictionary<string, List<Component>> notifications = new Dictionary<string, List<Component>>();

    // AddObserver includes a version where the observer can request to only receive notifications from a specific object.  We haven't implemented that yet, so the sender value is ignored for now.
    public void AddObserver(Component observer, string name) { AddObserver(observer, name, null); }
    public void AddObserver(Component observer, string name, Component sender)
    {
        // If the name isn't good, then throw an error and return.
        if (string.IsNullOrEmpty(name)) { Debug.Log("Null name specified for notification in AddObserver."); return; }
        // If this specific notification doesn't exist yet, then create it.
        if (!notifications.ContainsKey(name))
        {
            notifications.Add (name, new List<Component>());
        }

        List<Component> notifyList = notifications[name] as List<Component>;

        // If the list of observers doesn't already contain the one that's registering, then add it.
        if (!notifyList.Contains(observer)) { notifyList.Add(observer); }
    }


    public void RemoveObserver(Component observer, string name)
    {
        List<Component> notifyList = (List<Component>)notifications[name];

        // Assuming that this is a valid notification type, remove the observer from the list.
        // If the list of observers is now empty, then remove that notification type from the notifications hash. This is for housekeeping purposes.
        if (notifyList != null)
        {
            if (notifyList.Contains(observer)) { notifyList.Remove(observer); }
            if (notifyList.Count == 0) { notifications.Remove(name); }
        }
    }

    // PostNotification sends a notification object to all objects that have requested to receive this type of notification.
    // A notification can either be posted with a notification object or by just sending the individual components.

    public void PostNotification(Component aSender, string aName) { PostNotification(new Notification(aSender, aName)); }
    public void PostNotification(Component aSender, string aName, Hashtable aData) { PostNotification(new Notification(aSender, aName, aData)); }
    public void PostNotification(Component aSender, string aName, object eventArgs) { PostNotification(new Notification(aSender, aName, eventArgs)); }
    public void PostNotification(Notification aNotification)
    {
        // First make sure that the name of the notification is valid.
        //Debug.Log("sender: " + aNotification.name);
        if (string.IsNullOrEmpty(aNotification.name))
        {
            Debug.Log("Null name sent to PostNotification.");
            return;
        }
        // Obtain the notification list, and make sure that it is valid as well
        List<Component> notifyList = (List<Component>)notifications[aNotification.name];
        if (notifyList == null)
        {
            Debug.Log("Notify list not found in PostNotification: " + aNotification.name);
            return;
        }

        // Create an array to keep track of invalid observers that we need to remove
        List<Component> observersToRemove = new List<Component>();

        // Itterate through all the objects that have signed up to be notified by this type of notification.
        foreach (Component observer in notifyList)
        {
            // If the observer isn't valid, then keep track of it so we can remove it later.
            // We can't remove it right now, or it will mess the for loop up.
            if (!observer) { observersToRemove.Add(observer); }
            else
            {
                // If the observer is valid, then send it the notification. The message that's sent is the name of the notification.
                Debug.Log("Notified " + aNotification.name);
                observer.SendMessage(aNotification.name, aNotification, SendMessageOptions.DontRequireReceiver);
            }
        }

        // Remove all the invalid observers
        foreach (Component observer in observersToRemove)
        {
            notifyList.Remove(observer);
        }
    }

    public void RemoveRedundancies()
	{
		//Create new dictionary
		Dictionary<string, List<Component>> TmpNotifications = new Dictionary<string, List<Component>>();
			
		//Cycle through all dictionary entries
		foreach(KeyValuePair<string, List<Component>> Item in notifications)
		{
			//Cycle through all listener objects in list, remove null objects
			for(int i = Item.Value.Count-1; i>=0; i--)
			{
				//If null, then remove item
				if(Item.Value[i] == null)
					Item.Value.RemoveAt(i);
			}
			
			//If items remain in list for this notification, then add this to tmp dictionary
			if(Item.Value.Count > 0)
				TmpNotifications.Add (Item.Key, Item.Value);
		}
		
		//Replace listeners object with new, optimized dictionary
		notifications = TmpNotifications;
	}
	//------------------------------------------------
	//Called when a new level is loaded; remove redundant entries from dictionary; in case left-over from previous scene
	void OnLevelWasLoaded()
	{
		//Clear redundancies
		RemoveRedundancies();
	}


    // The Notification class is the object that is send to receiving objects of a notification type.
    // This class contains the sending GameObject, the name of the notification, and optionally a hashtable containing data.
    public class Notification
    {

        //public Notification (GameObject aSender, string aName, Hashtable aData)
        //{
        //	throw new System.NotImplementedException ();
        //}

        public Component sender;
        public string name;
        public Hashtable data;
        public object EventArgs;

        public Notification(Component aSender, string aName) { sender = aSender; name = aName; }
        public Notification(Component aSender, string aName, object eventArgs) { sender = aSender; name = aName; EventArgs = eventArgs; }
        public Notification(Component aSender, string aName, Hashtable aData) { sender = aSender; name = aName; data = aData; }
    }
}
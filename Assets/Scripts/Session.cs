using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Session {
    public int currentUserId;
    public User currentUser;

    public List<int> activeUsersId;
    public int usersCounter;
}

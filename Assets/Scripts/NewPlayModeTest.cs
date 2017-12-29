using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

public class PlayModeFirebaseTests {
    private string DatabaseUrl = "https://cs126-final-project.firebaseio.com/"; 
    private string P12FileName = "CS126 Final Project-5ede4958bb09.p12"; 
    private string ServiceAccountEmail = "firebase-adminsdk-sliqn@cs126-final-project.iam.gserviceaccount.com";
    
    //myUser is the user that is currently logged in and has read write authority
    private string myUser = "cg6acILlEgYMYTZ16npyROioWl02";

    //otherUser is not logged in and does not have read write authority
    private string otherUser = "cBMCg7NW9TVV0eTgTWeHKnHfmjA3";

    //Setup credentials to test in unity with firebase
    [SetUp]
    public void EditorLogin()
    {
        Debug.Log("Editor Login");
        FirebaseApp.DefaultInstance.SetEditorP12FileName(P12FileName);
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail(ServiceAccountEmail);
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DatabaseUrl);
        FirebaseApp.DefaultInstance.SetEditorAuthUserId("cg6acILlEgYMYTZ16npyROioWl02");
    }

    //Tasks are created in these functions and are then run in the YieldWait function

    //----------------------------Tasks---------------------------------------------
    private Task WriteScore(int score, string userId)
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).Child("highscore").SetValueAsync(score);
    }

    private Task<DataSnapshot> ReadScore(string userId)
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).Child("highscore").GetValueAsync();
    }

    private Task WriteName(string name, string userId)
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).Child("name").SetValueAsync(name);
    }


    private Task<DataSnapshot> ReadName(string userId)
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).Child("name").GetValueAsync();
    }

    //-------------------------Simple Write Score Tests---------------------------
    [UnityTest]
    public IEnumerator WriteScoreTest()
    {
        Task t = WriteScore(32, myUser);
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.Not.True);
    }

    [UnityTest]
    public IEnumerator ImproperUserWriteScoreTest()
    {
        Task t = WriteScore(32, "abcdefg");
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.True);
    }

    [UnityTest]
    public IEnumerator WriteToWrongUserScoreTest()
    {
        Task t = WriteScore(79, otherUser);
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.True);
    }

    //---------------------Set New High Score Tests-------------------------
    [UnityTest]
    public IEnumerator WriteNewHighScoreTest()
    {
        int myScore = 68;

        Task<DataSnapshot> readT = ReadScore(myUser);
        yield return YieldWaitTest(readT);

        DataSnapshot snapshot = readT.Result;
        int score = (int)(System.Int64)snapshot.Value;

        if(myScore > score)
        {
            Task writeT = WriteScore(myScore, myUser);
            yield return YieldWaitTest(writeT);
        
        }

        Task<DataSnapshot> readNewT= ReadScore(myUser);
        yield return YieldWaitTest(readNewT);

        DataSnapshot newSnapshot = readNewT.Result;
        score = (int)(System.Int64)newSnapshot.Value;

        Assert.AreEqual(myScore, score);
    }

    [UnityTest]
    public IEnumerator NoChangeHighScoreTest()
    {
        int myScore = 23;

        Task<DataSnapshot> readT = ReadScore(myUser);
        yield return YieldWaitTest(readT);

        DataSnapshot snapshot = readT.Result;
        int prevScore = (int)(System.Int64)snapshot.Value;

        if (myScore > prevScore)
        {
            Task writeT = WriteScore(myScore, myUser);
            yield return YieldWaitTest(writeT);

        }

        Task<DataSnapshot> readNewT = ReadScore(myUser);
        yield return YieldWaitTest(readNewT);

        DataSnapshot newSnapshot = readNewT.Result;
        int score = (int)(System.Int64)newSnapshot.Value;

        Assert.AreEqual(prevScore, score);
    }

    //-----------------Simple Read Score Tests--------------------

    [UnityTest]
    public IEnumerator ReadScoreTest()
    {
        Task<DataSnapshot> t = ReadScore(myUser);
        

        yield return YieldWaitTest(t);

        DataSnapshot snapshot = t.Result;
        int score = (int)(System.Int64)snapshot.Value;

        Assert.That(t.IsFaulted, Is.Not.True);
        Assert.AreEqual(32, score);
    }

    [UnityTest]
    public IEnumerator ReadScoreWrongUserTest()
    {
        Task<DataSnapshot> t = ReadScore(otherUser);
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.True);
    }

    //---------------Simple Write Name Tests---------------------

    [UnityTest]
    public IEnumerator WriteNameTest()
    {
        Task t = WriteName("bob", myUser);
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.Not.True);
    }

    [UnityTest]
    public IEnumerator WriteToWrongUserNameTest()
    {
        Task t = WriteName("jon", otherUser);
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.True);
    }

    //-------------------Simple Read Name Tests--------------------
    //Read own name
    [UnityTest]
    public IEnumerator ReadNameTest()
    {
        Task<DataSnapshot> t = ReadName(myUser);

        yield return YieldWaitTest(t);

        DataSnapshot snapshot = t.Result;
        string name = (string)(System.String)snapshot.Value;

        Assert.That(t.IsFaulted, Is.Not.True);
        Assert.AreEqual("bob", name);
    }


    //Function to run the task and wait for it to complete
    private static IEnumerator YieldWaitTest(Task task)
    {
        float timeTaken = 0;
        while (task.IsCompleted == false)
        {
            if (task.IsCanceled)
            {
                Assert.Fail("Task canceled!");
            }
            yield return null;
            timeTaken += Time.deltaTime;
            if (timeTaken > 5)
            {
                Assert.Fail("Time out!");
            }
        }
        Debug.Log("Task time taken : " + timeTaken);
    }

}
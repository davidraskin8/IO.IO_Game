using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

public class PlaymodeSetup : MonoBehaviour
{
    //This class is to setup the other player in firebase and initialize their data
    private string DatabaseUrl = "https://cs126-final-project.firebaseio.com/";
    private string P12FileName = "CS126 Final Project-5ede4958bb09.p12";
    private string ServiceAccountEmail = "firebase-adminsdk-sliqn@cs126-final-project.iam.gserviceaccount.com";

    //myUser is the user that is currently logged in and has read write authority
    private string myUser = "cBMCg7NW9TVV0eTgTWeHKnHfmjA3"; 

    //otherUser is not logged in and does not have read write authority
    private string otherUser = "cg6acILlEgYMYTZ16npyROioWl02";

    //Setup credentials to test in unity with firebase
    [SetUp]
    public void EditorLogin()
    {
        Debug.Log("Editor Login");
        FirebaseApp.DefaultInstance.SetEditorP12FileName(P12FileName);
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail(ServiceAccountEmail);
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DatabaseUrl);
        FirebaseApp.DefaultInstance.SetEditorAuthUserId("cBMCg7NW9TVV0eTgTWeHKnHfmjA3");
    }

    private Task WriteScore(int score, string userId)
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).Child("highscore").SetValueAsync(score);
    }

    private Task WriteName(string name, string userId)
    {
        return FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).Child("name").SetValueAsync(name);
    }

    [UnityTest]
    public IEnumerator WriteScoreTest()
    {
        Task t = WriteScore(1, myUser);
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.Not.True);
    }

    [UnityTest]
    public IEnumerator WriteNameTest()
    {
        Task t = WriteName("jason", myUser);
        yield return YieldWaitTest(t);
        Assert.That(t.IsFaulted, Is.Not.True);
    }

    //Run task and wait for it to finish
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

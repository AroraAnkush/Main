using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Linq;
using Facebook.Unity;
using Facebook.MiniJSON;

public class UIManager : Singleton<UIManager>
{
	//Screens
	public GameObject SplashScreen;
	public GameObject SignInMenu;
	public GameObject MainMenu;
	public GameObject InAppScreen;
	public GameObject ChooseAnteScreen;
	public GameObject BonusCodeScreen;
	public GameObject GamePlayScreen;
	public GameObject TableScene;
	public GameObject FacebookSignInScreen;
	public GameObject GoogleSignInScreen;
	public GameObject SignUpScreen;
	public GameObject SignInScreen;
	public GameObject ConfirmScreen;
	public GameObject GameOverScreen;
	public GameObject MyAccountScreen;
	public GameObject LoginQuestionScreen;
	public GameObject PreSignUp;
	public GameObject PracticeTable;
	
	
	public GameObject CurrentScreen;
	
	
	
	public UILabel playername;
	public UITexture playerimage;
	
	private bool mAuthenticating = false;
	private string mAuthProgressMessage = "Signing In and Loading in progress....";
	
	
	
	public bool Authenticating
	{
		get
		{
			return mAuthenticating;
		}
	}
	
	public bool Authenticated
	{
		get
		{
			return Social.Active.localUser.authenticated;
		}
	}
	
	public void SignOut()
	{
		((PlayGamesPlatform)Social.Active).SignOut();
	}
	
	public string AuthProgressMessage
	{
		get
		{
			return mAuthProgressMessage;
		}
	}
	
	
	
	public void Start()
	{
		FB.Init ();
		CurrentScreen = SplashScreen;
		
		CurrentScreen.SetActive(true);
		
	}
	
	
	public IEnumerator Splash_To_Menu()
	{
		yield return null;
		CurrentScreen.SetActive(false);
		CurrentScreen = SignInMenu;
		CurrentScreen.SetActive(true);
	}
	
	public void PlayPractice()
	{
		Application.LoadLevel(1);
	}
	
	public void OnSignIn()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = SignInScreen;
		CurrentScreen.SetActive(true);
	}
	
	public void OnSignUp()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = SignUpScreen;
		CurrentScreen.SetActive(true);
	}
	public void OnSignUpWithFB()
	{
		//        		CurrentScreen.SetActive (false);
		//        		CurrentScreen = SignInScreen;
		//        		CurrentScreen.SetActive (true);
		// if(!FB.IsLogged())FB.Login(FB.login_Native, OnLogin);
		CallFBLogin ();
	}
	
	public void Logout()
	{
		
		//        FB.Logout();
		CallFBLogout ();
		CurrentScreen.SetActive (false);
		CurrentScreen = SignInMenu;
		CurrentScreen.SetActive (true);
	}
	
	public void OnSignUpWithGoogle()
	{
		//        CurrentScreen.SetActive(false);
		//        CurrentScreen = SignInScreen;
		//        CurrentScreen.SetActive(true);
		Authenticate();
	}
	
	public void Authenticate()
	{
		if (Authenticated || mAuthenticating)
		{
			Debug.LogWarning("Ignoring repeated call to Authenticate().");
			return;
		}
		
		// Enable/disable logs on the PlayGamesPlatform
		//PlayGamesPlatform.DebugLogEnabled = GameConsts.PlayGamesDebugLogsEnabled;
		
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.EnableSavedGames()
				.Build();
		PlayGamesPlatform.InitializeInstance(config);
		
		// Activate the Play Games platform. This will make it the default
		// implementation of Social.Active
		PlayGamesPlatform.Activate();
		
		// Set the default leaderboard for the leaderboards UI
		// ((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(GameIds.LeaderboardId);
		
		// Sign in to Google Play Games
		mAuthenticating = true;
		Social.localUser.Authenticate((bool success) =>
		                              {
			mAuthenticating = false;
			if (success)
			{
				// if we signed in successfully, load data from cloud
				CurrentScreen.SetActive(false);
				CurrentScreen = MainMenu;
				CurrentScreen.SetActive(true);
				playername.text = Social.localUser.userName;
				playerimage.mainTexture = Social.localUser.image;
				Debug.Log("Login successful!");
				
			}
			else
			{
				// no need to show error message (error messages are shown automatically
				// by plugin)
				CurrentScreen = SignInMenu;
				CurrentScreen.SetActive(true);
				Debug.LogWarning("Failed to sign in with Google Play Games.");
			}
		});
	}
	
	public void OnSignInCancel()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = SignInMenu;
		CurrentScreen.SetActive(true);
	}
	public void OnSignInAccept()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	public void OnSignUpCancel()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = SignInMenu;
		CurrentScreen.SetActive(true);
	}
	public void OnSignUpAccept()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	
	public void OnQuickPlay()
	{
		// CurrentScreen.SetActive (false);
		CurrentScreen = ChooseAnteScreen;
		CurrentScreen.SetActive(true);
		
		// Application.LoadLevel(1);
	}
	public void OnTournament()
	{
		ChooseAnteScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	
	public void OnBuyLoad()
	{
		CurrentScreen.SetActive(false);
		MainMenu.SetActive(false);
		
		CurrentScreen = InAppScreen;
		CurrentScreen.SetActive(true);
	}
	
	public void OnBuyLoadCancel()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	
	public void OnPractice()
	{
		CurrentScreen.SetActive(false);
		MainMenu.SetActive(false);
		
		CurrentScreen = PracticeTable;
		CurrentScreen.SetActive(true);
	}
	
	public void OnTables()
	{
		CurrentScreen.SetActive(false);
		MainMenu.SetActive(false);
		
		CurrentScreen = TableScene;
		CurrentScreen.SetActive(true);
		
	}
	
	public void OnTableElement()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = GamePlayScreen;
		CurrentScreen.SetActive(true);
	}
	public void OnMyAccount()
	{
		CurrentScreen.SetActive(false);
		MainMenu.SetActive(false);
		CurrentScreen = MyAccountScreen;
		CurrentScreen.SetActive(true);
	}
	
	public void OnLowAnte()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	public void OnMedAnte()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	public void OnHighAnte()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	public void OnAnteScreenClose()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	
	
	public void OnLoginQuestionClicked()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = LoginQuestionScreen;
		CurrentScreen.SetActive(true);
		
	}
	
	public void OnQuestionOk()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = SignInMenu;
		CurrentScreen.SetActive(true);
	}
	
	public void OnAccountButtonBack()
	{
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	
	
	public void LeaveTable()
	{
		GameManager.Instance.OnConclusionCross();
		CurrentScreen.SetActive(false);
		CurrentScreen = MainMenu;
		CurrentScreen.SetActive(true);
	}
	
	public void Pre_Sign_Up()
	{
		
		CurrentScreen.SetActive(false);
		CurrentScreen = PreSignUp;
		CurrentScreen.SetActive(true);
	}
	
	
	
	/// //////////////////////////////////////
	/// fb
	/// ///////////////////////////////////////
	
	
	
	public void CallFBLogin()
	{
		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, OnInitComplete);
	}
	
	public void CallFBLoginForPublish()
	{
		// It is generally good behavior to split asking for read and publish
		// permissions rather than ask for them all at once.
		//
		// In your own game, consider postponing this call until the moment
		// you actually need it.
		FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, OnInitComplete);
	}
	
	public void CallFBLogout()
	{
		FB.LogOut();
	}
	
	public void OnInitComplete(ILoginResult result)
	{
		
		if (FB.IsLoggedIn && FB.IsInitialized && !result.Cancelled && result.Error == null) {
			
			FB.API("/me/picture", HttpMethod.GET, SetProfilePic);
			//FB.API("/me?fields=UserId", HttpMethod.GET, SetName);
			CurrentScreen.SetActive (false);
			CurrentScreen = MainMenu;
			CurrentScreen.SetActive (true);
			
		} else {
			CurrentScreen.SetActive (false);
			CurrentScreen = SignInMenu;
			CurrentScreen.SetActive (true);
			
		}
		
	}
	
	//	public void SetName(IAsyncResult result)
	//	{
	//		IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.ToString()) as IDictionary;
	//		playername.text = dict["first_name"].ToString();
	//	}
	public void SetProfilePic(IGraphResult result)
	{
		if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
		{
			
			playerimage.mainTexture = result.Texture;
		}
	}
	
	public void OnHideUnity(bool isGameShown)
	{
		//		mainmenuobj.Status = "Success - Check logk for details";
		//		mainmenuobj.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
		//		LogView.AddLog("Is game shown: " + isGameShown);
	}
}



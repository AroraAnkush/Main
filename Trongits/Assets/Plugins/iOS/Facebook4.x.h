//
//  Facebook4.x.h
//  FB4.x
//
//  Created by Keskil Egorov on 01/05/15.
//  Copyright (c) 2015 Keskil Egorov. All rights reserved.
//

#import <Foundation/Foundation.h>

NSString* const FBLogin_SystemAccount =     @"FBLogin_SystemAccount";       // login from System FB account. If not logged,
                                                                            // then flag will change to FBLogin_Native
NSString* const FBLogin_WebPopUp =          @"FBLogin_WebPopUp";            // login from UIWebView inside app
NSString* const FBLogin_Native =            @"FBLogin_Native";              // login from FB app. If FB app is not exist, then from Safari

extern bool debugMode;

#ifdef __cplusplus
extern "C" {
#endif

    //callback
    typedef void (*DATA_CALLBACK)(const char* key, const char* type, void* bytes, int length);
    typedef void (*MSG_CALLBACK_NEW)(const char* key, const char* type, const char* message);
    
    extern DATA_CALLBACK dataCallback;
    extern MSG_CALLBACK_NEW msgCallbackNew;
    
//    void _set_data_push(DATA_CALLBACK callback);
    void _set_msg_callback(MSG_CALLBACK_NEW callback);
    
    // Public functions
    bool _IsLogged();
    void _Login(const char* mode);
    void _Logout();
    
    #pragma mark - Graph API
    void _GraphRequest(const char* graphPath, const char* params, const char* method);
//    void _GraphPostVideo(const char* pTitle, const char* pDescription, const void* pVideoData, unsigned int pVideoDataSize);    // Available in only Pro version

    #pragma mark - Share
    void _SharePostWithImage(const char* pTitle, const void* pImageBytes, unsigned int pImageBytesLength);
    void _SharePostWithImageURL(const char* url, const char* title, const char* imageUrl, const char* descrp);
    void _ShareAPIWithImageURL(const char* url, const char* title, const char* imageUrl, const char* descrp);
    
    #pragma mark - FB Messenger
    bool _MessengerIsAvailable();
    void _MessengerShareLink(const char* url, const char* title, const char* imageUrl, const char* descrp);
    void _MessengerSharePhoto(const void* imageBytes, unsigned int imageBytesLength);
    
    #pragma mark - GameRequest, AppInvite
    void _GameRequest(const char* title, const char* message);
    void _AppInvite(const char* url, const char* previewImageUrl);
    
    #pragma mark
//    void _GetContentFromMediaLibrary();       // Available in only Pro version
    void _SetDebugMode(bool debugMode);
    void _PopUpWindow(const char* title, const char* message, int mode);
    
#ifdef __cplusplus
};
#endif


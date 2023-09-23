# YiQiKan.DotNet
### 公共请求头
>
```
Accept-Language: zh
Client-Type: Android
accessToken: {token}
Content-Type: application/json; charset=UTF-8
Connection: Keep-Alive
Accept-Encoding: gzip
User-Agent: okhttp/3.14.9
```
### 公共响应体
```JSON
{
    "data":{DATA},
    "currentTime": 1681110190,
    "resultCode": 200,
    "resultMsg": ""
}
```
# 1 域名  
## 1.1 域名获取
> https://tv-config100.oss-cn-hongkong.aliyuncs.com/api/v1.0/config/get  
> https://api.yqkapp.com/api/v1.0/link/getCheckDomainList  
> https://tv-config100.oss-accelerate.aliyuncs.com/api/v1.0/config/get  

以上三个固定的原始域名可以获取到动态更新的子域名  
【HttpGet】-https://tv-config100.oss-cn-hongkong.aliyuncs.com/api/v1.0/config/get 示例：
```JSON
{
    "apiUrlList": [
        {
            "url": "https://api-aws.11ty.top/",
            "isMustPass": 1,
            "checkType": "httpGet"
        },
        {
            "url": "https://hk-api-cf.yqk5659.com/",
            "isMustPass": 1,
            "checkType": "httpGet"
        },
        {
            "url": "https://api-aws.ctdy.cc/",
            "isMustPass": 0,
            "checkType": "httpGet"
        }
    ]
}
```
分别对apiUrlList中的所有域名进行请求，并判断最低延迟的域名，作为主域名   
【HttpGet】-{domain}api/v1.0/ping/get 示例：
```JSON
{
    "data": "v1.6.10",
    "resultMsg": "",
    "resultCode": 200,
    "currentTime": 1682305542199
}
```
<font color="red">(以上数据中data字段包含v1.x.x的内容，表示域名可以使用！) </font>

---
# 2 账号
## 2.1 用户名密码登录
> 【HttpPost】- {domain}api/v1.2/member/userLogin  

Body:
```JSON
{
    "areaCode":"86",
    "code":"",
    "latitude":0,
    "longitude":0,
    "model":"SM-G9980",
    "osVersion":"1.5.0",
    "passWord":"4ba0e2b3fa6d6a38b7b82583ae76c4e2",
    "phone":"{手机号}",
    "pushPackageName":"com.yiqikan.tv.mobile",
    "username":"{手机号}"
}
```

返回数据：
```JSON
{
    "user": {
        "userId": 10429704,
        "nickname": "大头",
        "userHead": "https://file2.yqk99.app/image/userDefaultHead/2022/09/29/10/20/c496cc62c076414389991d7313a62e63.png",
        "areaCode": "86",
        "phone": "132****3621",
        "sex": 1,
        "agentCode": "15FTGg",
        "inviteCode": "U1PQNKYA",
        "isFirstLogin": 0,
        "level": 0
    },
    "oAuth": {
        "token": "72b8952144834daabedfcd7e46f57478",
        "expiresIn": 18144000
    }
}
```

## 2.2 短信验证码登录
<br/>

### 2.2.1 先获取图形验证码（用来发送短信前的验证）
> 【HttpGet】- {domain}api/v1.0/phone/getCodeImg?areaCode=86&phone={手机号}&type=AutoLogin

返回数据：
是一个图片流，可以在浏览器直接打开查看  
<font color="red">每一次刷新这个链接，图形验证码都会改变，后续发短信时候填写的一定要是最新的图形验证码</font>
<br/>

### 2.2.2 发送短信验证码
> 【HttpPost】-{domain}api/v1.0/phone/send

Header：
["onlyIdentificationm":"xxxxxxxxxxxxxxxx"]
<font color="red">需要设置设备唯一标识</font>

Body:
```JSON
{
    "areaCode": "86",
    "areaCodeName": "中国大陆",
    "code": "bxn6",
    "isFromSetting": false,
    "phone": "{手机号}",
    "type": "AutoLogin"
}
```

返回数据：
```JSON
{
    "resultMsg":"",
    "resultCode":200,
    "currentTime":1682333086796
}
```
响应码为200即短信发送成功！

### 2.2.3 使用手机+短信验证码登录
> 【HttpPost】-{domain}api/v1.0/phone/vailSms

Header：
```C# 
"onlyIdentificationm":"xxxxxxxxxxxxxxxx"
```
<font color="red">需要设置设备唯一标识</font>

Body:
```JSON
{
    "areaCode": "86",
    "areaCodeName": "中国大陆",
    "code": "{短信验证码}",
    "inviteCode": "",
    "isFromSetting": false,
    "model": "{手机型号}",
    "osVersion": "{软件版本 例如：1.5.4}",
    "phone": "{手机号}",
    "type": "AutoLogin"
}
```

返回数据：
```JSON
{
    "user": {
        "userId": 10429704,
        "nickname": "大头",
        "userHead": "https://file2.yqk99.app/image/userDefaultHead/2022/09/29/10/20/c496cc62c076414389991d7313a62e63.png",
        "areaCode": "86",
        "phone": "132****3621",
        "sex": 1,
        "agentCode": "15FTGg",
        "inviteCode": "U1PQNKYA",
        "isFirstLogin": 0,
        "level": 0
    },
    "oAuth": {
        "token": "72b8952144834daabedfcd7e46f57478",
        "expiresIn": 18144000
    }
}
```




## 2.3 获取会员信息(从前面登录的响应中获取到token去请求会员信息)
> 【HttpGet】-{domain}api/v1.0/user/get

返回数据
```JSON
{
    "userId": 10429704,
    "nickname": "大头",
    "userHead": "https://file2.yqk99.app/image/userDefaultHead/2022/09/29/10/20/c496cc62c076414389991d7313a62e63.png",
    "areaCode": "86",
    "phone": "132****3621",
    "points": 0,
    "userLevelType": "Normal",
    "level": 0,
    "levelName": "普通会员",
    "everydayPlayLimit": 15,
    "everydayPlaySurplus": 15,
    "everydayPlayList": [],
    "sex": 1,
    "inviteCode": "U1PQNKYA",
    "inviteCount": 0,
    "inviteLevelCount": 1,
    "shareLink": "https://www.yqk.app?channelCode=15FTGg&inviteCode=U1PQNKYA",
    "shareContent": "一起看APP要你好看，永久免费在线观看，支持投屏、离线观影，网罗海内外影视资源！下载用戳↓↓↓ https://www.yqk.app?channelCode=15FTGg&inviteCode=U1PQNKYA",
    "agentCode": "15FTGg"
}
```

## 2.4 登录日志记录（猜测可能是为token保活，保持不过期）
每次打开软件或者登录后，都会请求记录一下登录信息
> 【HttpPost】-{domain}api/v1.0/userInfoLog/savePhoneLoginLog

Body:
``` JSON
{
    "agentCode":"{账号的邀请码}",
    "onlyIdentificationm":"{设备唯一标识}",
    "ts":"{时间戳}",
    "sign":"{请求签名}"
}
```

<font color="red">sign签名计算方式如下：  </font>
1. 计算签名的过程中需要用到AES加密和SHA256加密  
2. 准备一个key：`"9JFPeqizqMhOGX1t"`  
3. 准备一个iv：`"A-16-Byte-String"`  
4. 拼接字符串：`$"agentCode={邀请码}&onlyIdentificationm={设备唯一标识}&ts={时间戳}"`  
5. 将第4步得到的字符串，以上述2和3的key、iv一起做Aes加密算法  
6. 将第5步得到的Aes数据，使用SHA256再进行加密  
7. 将第6步的数据 byte -- string转为字符串，即为签名结果

## 2.5 修改密码
> 【HttpPost】-{domain}api/v1.0/security/changePwd

Body:
```JSON
{
    "oldPassword":"xxxxxxxxxxx",
    "newPassword":"yyyyyyyyyyy",
}
```
<font color="red">新旧密码需要进行MD5加密 </font>

返回数据
```JSON
{
    "resultMsg":"",
    "resultCode":200,
    "currentTime":1682333086796
}
```
响应码为200即修改密码成功

## 2.6 修改昵称
> 【HttpPost】-{domain}api/v1.0/userSetting/updateName?nickName={新的昵称}

返回数据
```JSON
{
    "resultMsg":"",
    "resultCode":200,
    "currentTime":1682333086796
}
```
响应码为200即修改昵称成功

## 2.7 退出登录
> 【HttpPost】- {domain}api/v1.0/member/logout  

Body：
```JSON
{
    "deviceType":"android"
}
```
返回数据
```JSON
{
    "resultMsg":"",
    "resultCode":200,
    "currentTime":1682333086796
}
```
响应码为200即退出登录成功

---
# 3 视频
## 3.1 获取tab分类的视频推荐数据
> 【HttpGet】-{domain}api/v1.1/movie/getHomeList?category={分类参数}  

|参数|描述|
|:---|:----:|
|Recommend|推荐|
|Film|电影|
|TVSeries|电视剧|
|America|美剧|
|Korea|韩剧|
|VarietyShow|综艺|
|Anime|动漫|
|Documentary|纪录片|


返回数据
```JSON
{
    "topBanners": [
        {
            "id": "6478a1e19d6b3c4bf1d8283b",
            "title": "花戎",
            "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/44/3e38bf1073164a2ba2c04b98fd6cbfa9.jpg",
            "picJuHe": "",
            "type": "MovieHead",
            "sort": 1,
            "reminder": "",
            "movieId": "64787ddb9d6b3c4bf1d7a965"
        },
        {
            "id": "627a4a4d14919c347922b8f5",
            "title": "云襄传",
            "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/05/01/23/17/1ceb0121f538422392d82893a8dc227a.jpg",
            "picJuHe": "",
            "type": "MovieHead",
            "sort": 2,
            "reminder": "",
            "movieId": "644fd78973f0ff5221391183"
        },
        {
            "id": "627a4af114919c347922c0ad",
            "title": "护心",
            "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/05/10/10/31/097e195b4a0e4b9b842d4b5e45dd3ccd.jpg",
            "picJuHe": "",
            "type": "MovieHead",
            "sort": 3,
            "reminder": "",
            "movieId": "645a286e73f0ff5221561494"
        },
        {
            "id": "6412e63973f0ff5221465ad1",
            "title": "长月烬明",
            "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/04/12/11/52/746bd302572f4a4fad9698a98f55ab90.jpg",
            "picJuHe": "",
            "type": "MovieHead",
            "sort": 4,
            "reminder": "",
            "movieId": "642e9d7673f0ff5221d7a56d"
        },
        {
            "id": "636e07c982caf657555d1e34",
            "title": "他是谁",
            "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/03/16/15/31/9ddd6dfdebf54affa4d8490fd35ac02b.jpg",
            "picJuHe": "",
            "type": "MovieHead",
            "sort": 5,
            "reminder": "",
            "movieId": "641147b973f0ff522140d542"
        }
    ],
    "adMarquee": [
        {
            "id": "63fef20d87d4410e4fd8e037",
            "reminder": "亲爱的用户您好，欢迎使用YQK一起看影视APP",
            "type": "WebUrl",
            "linkAddress": "",
            "sort": 0
        },
        {
            "id": "63fef28387d4410e4fd8e515",
            "reminder": "目前正在进行系统维修当中，故有时会无法正常播放",
            "type": "WebUrl",
            "linkAddress": "",
            "sort": 1
        },
        {
            "id": "63fef2ea87d4410e4fd8ec6d",
            "reminder": "如遇无法正常播放请先点击视频下方进行【收藏】",
            "type": "WebUrl",
            "linkAddress": "",
            "sort": 2
        },
        {
            "id": "63fef3ab87d4410e4fd90dd3",
            "reminder": "我们会继续努力优化，让您有更好的观影体验",
            "type": "WebUrl",
            "linkAddress": "",
            "sort": 3
        },
        {
            "id": "63fef3c787d4410e4fd91847",
            "reminder": "感谢您的支持与喜爱，祝您观影愉快，生活顺心",
            "type": "WebUrl",
            "linkAddress": "",
            "sort": 4
        },
        {
            "id": "6357502f2d20bd0ffa7f5c43",
            "reminder": "《护心》呆萌小奶龙要当爹，会说情话会撩人",
            "type": "MovieHead",
            "movieId": "645a286e73f0ff5221561494",
            "sort": 5
        },
        {
            "id": "635207112d20bd0ffa641de6",
            "reminder": "《照亮你》心机婊挑拨离间，断送靳时川表白大业",
            "type": "MovieHead",
            "movieId": "6479d8c69d6b3c4bf1dda95c",
            "sort": 6
        },
        {
            "id": "6352071e2d20bd0ffa641fa2",
            "reminder": "《白色城堡》童年阴影需要用一生去治愈，太揪心了",
            "type": "MovieHead",
            "movieId": "64773c289d6b3c4bf1d3b82c",
            "sort": 7
        },
        {
            "id": "635207022d20bd0ffa641cfa",
            "reminder": "《长月烬明》魔神和仙子的爱恨传说",
            "type": "MovieHead",
            "movieId": "642e9d7673f0ff5221d7a56d",
            "sort": 8
        },
        {
            "id": "635135532d20bd0ffa629d3c",
            "reminder": "《微雨燕双飞》当下是姊妹情深，往后终是意难平",
            "type": "MovieHead",
            "movieId": "647dc05b9d6b3c4bf1e6b0db",
            "sort": 9
        }
    ],
    "adInListBanners": [
        {
            "typeName": "相关推荐",
            "recommendDtos": [
                {
                    "id": "6479724a9d6b3c4bf1da00e6",
                    "recommend": "Recommend",
                    "module": "相关推荐",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/38/a7e3ec2a0a0b4c8992dcf4e9dc951e9a.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "64787ddb9d6b3c4bf1d7a965"
                },
                {
                    "id": "647972029d6b3c4bf1d9fdcd",
                    "recommend": "Recommend",
                    "module": "相关推荐",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/37/4cea283bd3d843e281bf44ec52a3bec0.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "644fd78973f0ff5221391183"
                },
                {
                    "id": "647971db9d6b3c4bf1d9fb37",
                    "recommend": "Recommend",
                    "module": "相关推荐",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/36/21f294645d314120a4deac780542ec74.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "645a286e73f0ff5221561494"
                }
            ]
        },
        {
            "typeName": "重磅热播",
            "recommendDtos": [
                {
                    "id": "6479724a9d6b3c4bf1da00da",
                    "recommend": "Recommend",
                    "module": "重磅热播",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/38/a7e3ec2a0a0b4c8992dcf4e9dc951e9a.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "64787ddb9d6b3c4bf1d7a965"
                },
                {
                    "id": "647972029d6b3c4bf1d9fdc1",
                    "recommend": "Recommend",
                    "module": "重磅热播",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/37/4cea283bd3d843e281bf44ec52a3bec0.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "644fd78973f0ff5221391183"
                },
                {
                    "id": "647971db9d6b3c4bf1d9fb2b",
                    "recommend": "Recommend",
                    "module": "重磅热播",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/36/21f294645d314120a4deac780542ec74.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "645a286e73f0ff5221561494"
                }
            ]
        },
        {
            "typeName": "最近更新",
            "recommendDtos": [
                {
                    "id": "6479724a9d6b3c4bf1da00dd",
                    "recommend": "Recommend",
                    "module": "最近更新",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/38/a7e3ec2a0a0b4c8992dcf4e9dc951e9a.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "64787ddb9d6b3c4bf1d7a965"
                },
                {
                    "id": "647972029d6b3c4bf1d9fdc4",
                    "recommend": "Recommend",
                    "module": "最近更新",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/37/4cea283bd3d843e281bf44ec52a3bec0.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "644fd78973f0ff5221391183"
                },
                {
                    "id": "647971db9d6b3c4bf1d9fb2e",
                    "recommend": "Recommend",
                    "module": "最近更新",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/36/21f294645d314120a4deac780542ec74.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "645a286e73f0ff5221561494"
                }
            ]
        },
        {
            "typeName": "Netflix新片动态",
            "recommendDtos": [
                {
                    "id": "6479724a9d6b3c4bf1da00e3",
                    "recommend": "Recommend",
                    "module": "Netflix新片动态",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/38/a7e3ec2a0a0b4c8992dcf4e9dc951e9a.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "64787ddb9d6b3c4bf1d7a965"
                },
                {
                    "id": "647972029d6b3c4bf1d9fdca",
                    "recommend": "Recommend",
                    "module": "Netflix新片动态",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/37/4cea283bd3d843e281bf44ec52a3bec0.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "644fd78973f0ff5221391183"
                },
                {
                    "id": "647971db9d6b3c4bf1d9fb34",
                    "recommend": "Recommend",
                    "module": "Netflix新片动态",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/36/21f294645d314120a4deac780542ec74.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "645a286e73f0ff5221561494"
                }
            ]
        },
        {
            "typeName": "一周热点",
            "recommendDtos": [
                {
                    "id": "6479724a9d6b3c4bf1da00e0",
                    "recommend": "Recommend",
                    "module": "一周热点",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/38/a7e3ec2a0a0b4c8992dcf4e9dc951e9a.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "64787ddb9d6b3c4bf1d7a965"
                },
                {
                    "id": "647972029d6b3c4bf1d9fdc7",
                    "recommend": "Recommend",
                    "module": "一周热点",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/37/4cea283bd3d843e281bf44ec52a3bec0.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "644fd78973f0ff5221391183"
                },
                {
                    "id": "647971db9d6b3c4bf1d9fb31",
                    "recommend": "Recommend",
                    "module": "一周热点",
                    "imgUrl": "https://file2.yqk99.app/image/tvBanner/2023/06/02/12/36/21f294645d314120a4deac780542ec74.png",
                    "picJuHe": "",
                    "type": "MovieHead",
                    "movieId": "645a286e73f0ff5221561494"
                }
            ]
        }
    ],
    "isHaveTopBanners": true,
    "resources": [
        {
            "typeName": "重磅热播",
            "datalist": [
                {
                    "movieId": "63c2c5676a582a76ce01c90e",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/04/12/18/14/8afe81f647b04662ac8c4ddcb0d2c0b3.jpg",
                    "picJuHe": "",
                    "name": "狂飙",
                    "tvNumber": 39,
                    "serializeStatus": 1,
                    "score": 8.5,
                    "category": "TVSeries",
                    "modifyTime": 1682180744949,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "646b4b1f9d6b3c4bf1b5a1c2",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/26/16/26/63080a44641246c2819576a861a8a7df.jpg",
                    "picJuHe": "",
                    "name": "三分野",
                    "tvNumber": 32,
                    "score": 6.3,
                    "category": "TVSeries",
                    "modifyTime": 1686194594869,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "64787ddb9d6b3c4bf1d7a965",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/01/19/14/b2426a60694c4e0facbddbfdc235c1d5.jpg",
                    "picJuHe": "",
                    "name": "花戎",
                    "tvNumber": 26,
                    "serializeStatus": 0,
                    "category": "TVSeries",
                    "modifyTime": 1686194582113,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "645a286e73f0ff5221561494",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/10/16/51/0a1ead5d71c4416484bf55de544410fb.jpg",
                    "picJuHe": "",
                    "name": "护心",
                    "tvNumber": 40,
                    "serializeStatus": 1,
                    "score": 7.1,
                    "category": "TVSeries",
                    "modifyTime": 1686194558635,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "6460e7cd73f0ff5221692b45",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/19/15/10/7057e0cbf8a74b1a948e30fd2f6334f4.jpg",
                    "picJuHe": "",
                    "name": "后浪",
                    "tvNumber": 40,
                    "serializeStatus": 1,
                    "category": "TVSeries",
                    "modifyTime": 1686194543928,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "647894c59d6b3c4bf1d802f1",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/07/16/02/671809fb664242e4898c1c264017e989.jpg",
                    "picJuHe": "",
                    "name": "梦中的那片海",
                    "tvNumber": 24,
                    "serializeStatus": 0,
                    "category": "TVSeries",
                    "modifyTime": 1686194527217,
                    "isMovie": false,
                    "updateTime": false
                }
            ]
        },
        {
            "typeName": "最近更新",
            "datalist": [
                {
                    "movieId": "648412dd9d6b3c4bf1f63547",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/10/14/05/2a135cae469341f0a1fa0baa914ad1b6.jpg",
                    "picJuHe": "",
                    "name": "昆仑境",
                    "tvNumber": 1,
                    "category": "Film",
                    "modifyTime": 1686536559341,
                    "isMovie": true,
                    "updateTime": true
                },
                {
                    "movieId": "648300279d6b3c4bf1f3bf45",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/09/18/34/6739f26e618c40a5a4f726f5d67b6c3f.png",
                    "picJuHe": "",
                    "name": "猎犬",
                    "tvNumber": 8,
                    "category": "TVSeries",
                    "modifyTime": 1686536541521,
                    "isMovie": false,
                    "updateTime": true
                },
                {
                    "movieId": "6482b1b89d6b3c4bf1f268f4",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/09/12/58/7dc2fd49f7d6488193b811e6d10fee13.jpg",
                    "picJuHe": "",
                    "name": "心跳",
                    "tvNumber": 14,
                    "serializeStatus": 0,
                    "category": "TVSeries",
                    "modifyTime": 1686536521175,
                    "isMovie": false,
                    "updateTime": true
                },
                {
                    "movieId": "6481a9229d6b3c4bf1f03823",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/08/18/09/6ab6afea001a404c8ebb60c7f0395bf6.jpg",
                    "picJuHe": "",
                    "name": "好想做一次 第四季 Never Have I Ever Season 4",
                    "tvNumber": 10,
                    "serializeStatus": 1,
                    "score": 8.8,
                    "category": "TVSeries",
                    "modifyTime": 1686278554686,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "64816f539d6b3c4bf1ef51e0",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/12/16/37/5548fa54ceb841bd99b1950cf78fe618.jpg",
                    "picJuHe": "",
                    "name": "良辰美景又逢君",
                    "tvNumber": 10,
                    "serializeStatus": 0,
                    "category": "TVSeries",
                    "modifyTime": 1686278565535,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "648209b89d6b3c4bf1f0f245",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/09/01/02/b0b6750c553d46e0b923f33257337e96.jpg",
                    "picJuHe": "",
                    "name": "变形金刚：超能勇士崛起 Transformers: Rise of the Beasts",
                    "tvNumber": 1,
                    "serializeStatus": 0,
                    "category": "Film",
                    "modifyTime": 1686278421542,
                    "isMovie": true,
                    "updateTime": false
                }
            ]
        },
        {
            "typeName": "一周热点",
            "datalist": [
                {
                    "movieId": "64524f7673f0ff522140b4b8",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/03/20/10/47e146012ea24f80ad3e5f8bd2275a1f.jpg",
                    "picJuHe": "",
                    "name": "平凡之路",
                    "tvNumber": 36,
                    "serializeStatus": 1,
                    "score": 7.2,
                    "category": "TVSeries",
                    "modifyTime": 1686194851598,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "646de49b9d6b3c4bf1bee593",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/27/16/39/ba98f985d7484e849c1e1b8d0d4fd564.jpg",
                    "picJuHe": "",
                    "name": "妻子的新世界",
                    "tvNumber": 24,
                    "serializeStatus": 1,
                    "category": "TVSeries",
                    "modifyTime": 1686194839957,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "644fd78973f0ff5221391183",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/01/23/13/f1c09eb55ad243ae801743f52a3f3d3c.jpg",
                    "picJuHe": "",
                    "name": "云襄传",
                    "tvNumber": 36,
                    "serializeStatus": 1,
                    "score": 7.3,
                    "category": "TVSeries",
                    "modifyTime": 1686194822714,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "642e9d7673f0ff5221d7a56d",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/04/12/17/33/2d8eff7c2cca48f5a443ad98f2364271.jpg",
                    "picJuHe": "",
                    "name": "长月烬明",
                    "tvNumber": 40,
                    "serializeStatus": 1,
                    "score": 5.6,
                    "category": "TVSeries",
                    "modifyTime": 1686194808338,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "620fe570f0dcbc757f7158e5",
                    "pic": "https://file2.yqk99.app/image/movieCover/2022/02/23/21/29/60b9fbb33ea84e6db3af571621bf8a77.jpg",
                    "picJuHe": "http://file1.yqkapp.com/item/621636aa2ab3f51d91287589.jpg",
                    "name": "如懿传",
                    "tvNumber": 87,
                    "serializeStatus": 1,
                    "score": 7.5,
                    "category": "TVSeries",
                    "modifyTime": 1686194795751,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "646b4d3e9d6b3c4bf1b5b064",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/26/17/10/736c0e0fd38c41ec9b3b6839bf9af2eb.jpg",
                    "picJuHe": "",
                    "name": "在下李佑",
                    "tvNumber": 26,
                    "serializeStatus": 0,
                    "category": "TVSeries",
                    "modifyTime": 1686194740851,
                    "isMovie": false,
                    "updateTime": false
                }
            ]
        },
        {
            "typeName": "Netflix新片动态",
            "datalist": [
                {
                    "movieId": "6478a1009d6b3c4bf1d818a4",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/06/16/36/3d37359aac5d4ce997642577f2d4b4d9.jpg",
                    "picJuHe": "",
                    "name": "美丽人生 A Beautiful Life",
                    "tvNumber": 1,
                    "serializeStatus": 0,
                    "score": 6.3,
                    "category": "Film",
                    "modifyTime": 1686194952243,
                    "isMovie": true,
                    "updateTime": false
                },
                {
                    "movieId": "64787cff9d6b3c4bf1d7a018",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/06/16/36/4d414acb2fe7453c83606fdc2fb7850a.jpg",
                    "picJuHe": "",
                    "name": "核灾日月",
                    "tvNumber": 8,
                    "serializeStatus": 1,
                    "score": 7.3,
                    "category": "TVSeries",
                    "modifyTime": 1686194938404,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "6477196d9d6b3c4bf1d34eeb",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/06/16/25/d234489d1b9349d9831852b24c1d9504.jpg",
                    "picJuHe": "",
                    "name": "混然天成",
                    "tvNumber": 1,
                    "score": 6.2,
                    "category": "Film",
                    "modifyTime": 1686194924963,
                    "isMovie": true,
                    "updateTime": false
                },
                {
                    "movieId": "6470a8b29d6b3c4bf1c5392c",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/02/15/36/9e1a113f47f84a889464384e7d1bd383.jpg",
                    "picJuHe": "",
                    "name": "血黄金 Blood and Gold",
                    "tvNumber": 1,
                    "score": 6.2,
                    "category": "Film",
                    "modifyTime": 1686194912629,
                    "isMovie": true,
                    "updateTime": false
                },
                {
                    "movieId": "64709ec09d6b3c4bf1c5348a",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/01/16/04/d6e9f3a710e54dc2b833b65627c57d99.jpg",
                    "picJuHe": "",
                    "name": "双生谜 Tin & Tina",
                    "tvNumber": 1,
                    "score": 5.1,
                    "category": "Film",
                    "modifyTime": 1686194899898,
                    "isMovie": true,
                    "updateTime": false
                },
                {
                    "movieId": "646e2c2d9d6b3c4bf1bfe0e9",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/27/16/42/f5118200043449148d396ca74aff7441.jpg",
                    "picJuHe": "",
                    "name": "青春硬起来",
                    "tvNumber": 1,
                    "score": 5.3,
                    "category": "Film",
                    "modifyTime": 1686194888122,
                    "isMovie": true,
                    "updateTime": false
                }
            ]
        },
        {
            "typeName": "相关推荐",
            "datalist": [
                {
                    "movieId": "641147b973f0ff522140d542",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/03/15/12/20/8ec3525a12054da48005153f5b129b7b.jpg",
                    "picJuHe": "",
                    "name": "他是谁",
                    "tvNumber": 24,
                    "serializeStatus": 1,
                    "score": 8.2,
                    "category": "TVSeries",
                    "modifyTime": 1686195059104,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "6425a1be73f0ff5221b68477",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/04/12/18/15/bc36a28004fa48d384c635eca9c8a345.jpg",
                    "picJuHe": "",
                    "name": "无间",
                    "tvNumber": 40,
                    "serializeStatus": 1,
                    "score": 5.4,
                    "category": "TVSeries",
                    "modifyTime": 1686195043933,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "642eb41973f0ff5221d7e97f",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/04/06/19/58/2cee8707c1004b02b4520e710db1e6a4.jpg",
                    "picJuHe": "",
                    "name": "尘封十三载",
                    "tvNumber": 24,
                    "serializeStatus": 1,
                    "score": 8.1,
                    "category": "TVSeries",
                    "modifyTime": 1686195027943,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "642daf0673f0ff5221d2f308",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/04/11/18/04/b8692c1ec24c43a0baa7bd6815044ae6.jpg",
                    "picJuHe": "",
                    "name": "薄冰",
                    "tvNumber": 40,
                    "serializeStatus": 1,
                    "score": 5.5,
                    "category": "TVSeries",
                    "modifyTime": 1686195010441,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "6372285482caf65755780c75",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/04/12/18/11/615e4dc6f73f48bab1afed8fcf7a5723.jpg",
                    "picJuHe": "",
                    "name": "爱情而已",
                    "tvNumber": 38,
                    "serializeStatus": 1,
                    "score": 8.3,
                    "category": "TVSeries",
                    "modifyTime": 1686194999003,
                    "isMovie": false,
                    "updateTime": false
                },
                {
                    "movieId": "610d8e0058c30b019f1eefad",
                    "pic": "https://file2.yqk99.app/image/movieCover/2023/05/12/18/13/4a6cde8940ab4ea1acc7c499c0bf8765.jpg",
                    "picJuHe": "",
                    "name": "庆余年 第一季",
                    "tvNumber": 46,
                    "serializeStatus": 1,
                    "score": 7.9,
                    "category": "TVSeries",
                    "modifyTime": 1686194987231,
                    "isMovie": false,
                    "updateTime": false
                }
            ]
        }
    ]
}
```

## 3.2 获取视频详情
> 【HttpGet】- {domain}api/v1.1/movie/getDetail?movieId={视频id}objectId={objID}

``视频ID从``其他的``视频列表信息``中找到
``ObjID未知``

返回数据：
```JSON
{
    "movieId": "648f196a9d6b3c4bf112621c",
    "pic": "https://file2.yqk99.app/image/movieCover/2023/06/19/16/35/10c3ab2371dd48279cb77c04c3b95b77.jpg",
    "name": "长风渡",
    "classType": "剧情,古装",
    "area": "China",
    "year": "2023",
    "introduction": "　　布商之女柳玉茹自小遭遇生母病重、庶母不慈、父亲不重视的困境，她不得已谨小慎微，做了十几年模范闺秀。不料，对婚姻抱有憧憬的她却被家里安排嫁给了名满扬州的纨绔顾九思。顾九思误会柳玉茹是为了攀附权贵才嫁给他，对其嗤之以鼻。柳玉茹幡然醒悟，不能将命运系之婚姻、系之他人，决定跟随顾母学习经商之道，真实、独立、精彩地实现自我价值。经过努力，柳玉茹在生意上逐渐得心应手，更在此过程中发现了顾九思至纯至善、炽热真诚的一面，两个人的心也越靠越近。然而此时，扬州节度使倒行逆施，导致百姓流离失所，顾家不得已举家逃亡。顾九思看着万千流民，一夜之间长大。为结束乱局，让百姓安居，顾九思从捕快做起，官至户部尚书，与志同道合者扫除积弊、轻税轻徭。柳玉茹则内修善堂、外建商交，使得物资繁盛、百姓安康，两人共同谱写了一段盛世佳话。",
    "director": "尹涛",
    "actress": "白敬亭,宋轶,刘学义,张昊唯,张睿,赵子琪,张绍刚,沙溢,胡可,何晟铭,张棪琰,何中华,王策,吴冕,章呈赫,孔琳,程梓,王九胜,张晞临,丁勇岱,王瑞子,张维娜,李欣泽,赵圆瑗,聂子皓,艾米,李小胖,郭昊钧,赵奂然,邓凯",
    "category": "TVSeries",
    "serializeStatus": 1,
    "tvNumber": 40,
    "isUpdateDouBan": true,
    "watchCount": 1417681,
    "distinctWatchCount": 430718,
    "actors": "[{\"name\":\"尹涛\",\"role\":\"导演\",\"headImg\":\"https://img1.doubanio.com/view/celebrity/raw/public/p1597128183.07.jpg\"},{\"name\":\"白敬亭\",\"role\":\"演员\",\"headImg\":\"https://img2.doubanio.com/view/personage/raw/public/956c3479db29c846cf17aa085d6a9a5f.jpg\"},{\"name\":\"宋轶\",\"role\":\"演员\",\"headImg\":\"https://img1.doubanio.com/view/personage/raw/public/802fd4743942ad8511d7671b544310a8.jpg\"},{\"name\":\"刘学义\",\"role\":\"演员\",\"headImg\":\"https://img1.doubanio.com/view/celebrity/raw/public/p1586426420.18.jpg\"},{\"name\":\"张昊唯\",\"role\":\"演员\"},{\"name\":\"张睿\",\"role\":\"演员\",\"headImg\":\"https://img2.doubanio.com/view/celebrity/raw/public/p48632.jpg\"}]",
    "score": 0,
    "playTipsType": "HasCount",
    "playTips": "",
    "everydayPlaySurplus": 15,
    "everydayPlayLimit": 15,
    "everydayPlayList": [],
    "shareContent": "《长风渡》高清播放,请点击立即在线观看 https://m.yqktv888.com/dl/index.html?sc=1001_oldyqk?channelCode=15FTGg&movieId=648f196a9d6b3c4bf112621c&inviteCode=U1PQNKYA",
    "resources": [
        {
            "name": "YQK",
            "typeName": "General",
            "datalist": [
                {
                    "name": "第01集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoIQHuswbzf1sOz9q8eLM0hlqLUh+ndZcxAd+Ra6xc00cw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第02集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoIcGrT9SdqYxdZAENsVX8tX0pNIi9onwZttoKC0Z5S8gA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第03集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoL10q2R10rsYInw4v8VfuKnAB0hK+zRe/rUQG7X2sxhpQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第04集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoIHKTORqy4c9kOsD5h/8JO2pt7UeU5KTrmxg4YEFMCI0g==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第05集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoJkp5YXflOaSJDUmqNm/jtlZnFvNy9QdmKdMFnv0hF1mQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第06集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoKcmmUMsyQqhEn67ezc3XXY09nvuquNI/6cKpYP0kH66w==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第07集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoLuyU2L5RUsTE445PbGiHoWVtvcgRY2G9JzOFb+AsZMBA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第08集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoKius62So1wpIJoo2RPm1AyAnfiVc/y6TmjTQUeZ6ZK9A==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第09集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoIfy91IJo7JteV0Lvx6qHmgqwaGaBnRjtsvaa16SSQ0yA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第10集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoJ5pHJfVk7LFSR5KHKr4kIplBGKnEmOYioV/tth+P5pYw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第11集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoKR43zxf+MQWCl2ggYjfAjAxamPaFhi9WLFA1sT4EWFhA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第12集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoJ1xwj90Hj0o4CLtXqZfhUT03XmKFqem+zFG0336ZBxLA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第13集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoLLhoYFWE853yqJKI5wUeSARLOA7MHUIKKOYIPV5o+wSQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第14集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoL8vveRe1I6zLNoUlIVrc44Oe5fldn+6VtFcpLLmi60pg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第15集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoL56yKxhv5/LWgdXo6kanheVjB7C3qh4dO2SiKujKfLnw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第16集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXK82R1FJLmwqrXqlas81hoIKQkQiIo/edbd7wZei0W5nI/V53voJma78voCo5zZrsw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第17集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXyJbAvLhodWzRbDxowsZUkogkZISpVntD7yx1J0c57BoQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第18集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXzKG9YLmvRuMaLuXkHkP/tjiNVJNOOXxWBV+VIPQ4mXaw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第19集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXzcKVeIkf81TuIFS9hTm2I+WhNEq8tJicN6JoWdKJ26fA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第20集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXyGtpcD+bmL9s+7UtB9MWUM/Gc2moaKRJ9JV5Tg8Aqywg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第21集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXwxhjkIxz20BPrz7cJ5g+38V6yDAZFCKAmIquY72p6dqQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第22集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXzKMzlqy4rNjdN+DK0WgRe6n/IPh3VJp8SylMBc/JCn9w==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第23集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXyYmX0NahcGT6UIJ49cDy2nla2IY3q6wB689AZQYkoqpQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第24集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXz40vBmicfVaLifFzaquTV7Az0FqW1yort0jaGU7qXNww==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第25集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXyt1xJHIFtX1/czTIF34qPGc6QYgMmlXD7Oeas/bwWLEQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第26集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXxEQRmwzPwR0V9VRjoM/Cvi5QHxPk9tMn1TIwAPH8HrwQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第27集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXxWCI1GDcYxL9tNOwNzSKhdyWjuutFU/x618SGF3lor8w==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第28集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXzQn9H2SURKbgGhZhDhoCJWmlsDEXNPD1EciYwZ2CYNgQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第29集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXzFjUxMz+UvANxgwMtuWA3iP/HZqnw7E/dtiuXvKy0yYQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第30集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXxg44fS7J7UC6+IHMo3P4OBFbsEvdZk/AVGv+4HKejsGQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第31集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXzd5Q9fhHrcHhAgQEx56iqmNjxq3lFDOF95JqqPcTCicg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第32集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXz4JGhRAOQzK4hIeTEyqrvZ+vmMYTqi0AUouuO2eHEHFg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第33集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXzUjv0vctRtO4u0R+35vafSjXOLgqjySulsDL6wFsPwDQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第34集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXw6hciDI48yLlbFvH25pTD1hE0ZI7txH6AY0/Ct3PS6Eg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第35集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXyWaCZBoEoJ5nxmhivP8wQ8erJuK/DLOH4DctjZ98WtRg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第36集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXwjhCTHxOMpLJgwKwXKaf+JVV+xtO3peo6SpzElcubCag==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第37集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXwB0bz3kTaHxgwsoLJPbbbq0dn6SRE0Ru7JdcDA1effTA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第38集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXGFNagaRx02JDcGe0oTWWXw0iAzoHfZpH5Q+fiMcM/E0N0LRx1HVLwW0RfDKj0wfdg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第39集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXJZRp/wWvLNwEI1j6DLEYohALU/kaBgqEuEqHImfDIa2N84PjgzAtqHsNC58rxL27A==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第40集",
                    "playType": "Local",
                    "address": "+Whc/4qRCu1ukWm3YoszXJZRp/wWvLNwEI1j6DLEYohcAMhJ03qhRiH/0fWyD4ZU/F1K2gsK8HFhhredNiH+fw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                }
            ],
            "vipPlay": 1
        },
        {
            "name": "备用源1",
            "typeName": "BY1",
            "datalist": [
                {
                    "name": "第01集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwAKI5kcgOm/FEFBxEihq4O1TB3ztet6i72QwozPoub3WS8emJAP/ksXYTeEy/nlyYA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第02集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwAKI5kcgOm/FEFBxEihq4O3Kj66Udo5k2hljEMWalVaunFLnHCpSxi3/5SqeHjTiuw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第03集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwAKI5kcgOm/FEFBxEihq4O2uSKr553vOtT326EL9jBEqJuyqciXY1RQ0PfdFJPhnGA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第04集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwAKI5kcgOm/FEFBxEihq4O2BJEdTwJPqzjifVBIqJbdnVTfMUgxji1o06d46cm6gkQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第05集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwAKI5kcgOm/FEFBxEihq4O2KosxmSQowP3nwVNI3Ndg3aZU/uoPRxhLhR/T6UA8dOg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第06集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwAKI5kcgOm/FEFBxEihq4O3gvyhkwWFjXbnBY1549gBJMJx0gjjZxxfIub3WLD+meg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第07集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoM88W7/5BoukvOiqmu9eJCYgiaZDa+3EG9cbPzGQNPIKNjkdgFXHgtbaq2Tojv0Z5g==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第08集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoM88W7/5BoukvOiqmu9eJCawQl+6SXyrt/SunnuaGAX5bzjPi9z+zMLfPmpIbqXf2g==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第09集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwIZBcqgSPtzwBXJEefPfuC2bG2mFR6C9le2XXF3NuZJbEjRnHOrLKkiizEMz+TwYOg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第10集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwIZBcqgSPtzwBXJEefPfuC20tTLjyPb1lMbhntoRN7P2FskGFhOTCw9AIi/XI3tzrA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第11集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pS58LPdJR/37HBrVDD0fqDxBhewX9D242Ug6u2hCHr4tw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第12集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pTKMXLGNSyDUnsD/xhF1n3D6ikHa7xpePwgT+3/IYcCuw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第13集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwGAj19BFWXCVsD8s58RTd4/wiJKGRIGyHApgfgDY6jL6TGmzwf05UFPnQjlwrKizSg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第14集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwGAj19BFWXCVsD8s58RTd4+qOoKkt5S0wnXmuQ6FXe0JUkx8LfUBMjLAgEwfn0kcXA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第15集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pTh8XPyGD/Bf2aQzzkvVYEDh6LXsfJQLnxcvOMhJqPxWA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第16集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pQIzWMGixokaI83OKyocrJs3QAtE9058+n+GpV8ff4ZsA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第17集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwP5xZjoKXtxV5FliwV3qm7chBf8h6MarGrugFPUu5yRWvQPiDk+raHQfDqDdey9pSg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第18集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwP5xZjoKXtxV5FliwV3qm7ews8oo6pAgn5qNBUnQNVrz94I5PXhbx94n5mx7ZiyXuA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第19集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pSl+CCMD1aEXAJlvZpUcuItXwSq/uJtvrQf5cPd2gvYRA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第20集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pQh2U0FyQTao06xpC/6Pwt1K7M8LRJ78nG2WJqs3Op2Ag==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第21集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pQe8NYREEows0lXNG/xZ6OZZdsJ3r2/D+8WV9n0rIMCeg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第22集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoOA9TWXo22E4Lo97FYgV7pQnI09fGhR9v7aj9mmeQ0oIxt3NHP6vkna/4WhYVujwxw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第23集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwK2MDcKtBzfHhr6cEy8VdRSE2S67rK7quWbwqpIgwgsIAieRJaMnjG6Ukw1IktWPag==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第24集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwK2MDcKtBzfHhr6cEy8VdRTTvsTRm/gDkNXAkbWpe2KiWT5mYBubSC9kYoANQVZaoA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第25集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoDGcln8jdIM7XxBjWAgW0F01SC7Dv+PxK2oGxieBZb/O7gdsa4L9uxcFOPNmXigR0g==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第26集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoDGcln8jdIM7XxBjWAgW0F21PXm4khXGJBCGoyk8h0kU+777aCciXxuHMHTyOAiySQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第27集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwLytdp7Kw1XbaPokzLkim82ha5aZv1mflwpU3Lx8pDjp2d1NThyPg3kc2rnKnLn7LQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第28集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwLytdp7Kw1XbaPokzLkim8245zAvsjMYcSI8FM53p9U3mF2FvhLDDe0kPEFprss6fg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第29集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwLbEnbZxrmPsqCsdODDx7uQ4ei/60M+pm2EyxO00zs1yJfm19XdHHcTwBFvtzqqjNw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第30集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwLbEnbZxrmPsqCsdODDx7uSaXPUy4/PrdUWFlHCgqaipFOARtyVMwqqLPod3/SXE/Q==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第31集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoDGcln8jdIM7XxBjWAgW0F37XrNBzfyUQJQkGQYKNCx7ZBWly9u4xWql+OX5SazEMw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第32集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoDGcln8jdIM7XxBjWAgW0F25QBUKQyc1uraGEfidoARjHbxuhmw3fuelx1hcLnejyg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第33集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwMrQzKuINoEHh5qsQBeSPK0gEgE6dc5pIC/3xJ9QqWEhXAk68Py1N8LKBWCqa/przg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第34集",
                    "playType": "Local",
                    "address": "7LMpyJKitMpD4r98VK6RwMrQzKuINoEHh5qsQBeSPK2jr9ljqDhMDjdJb2qchgwyq+wtBrF55X9gyRaXsMrXJw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第35集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoDGcln8jdIM7XxBjWAgW0F1KsK8Six81XwCOm8/1d3OnHkUPfEa0f0E5DoFxO0gVjw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第36集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoDGcln8jdIM7XxBjWAgW0F07N0+Gmlk1ywlhOGnkzZ4uvEB8r9bwEGilUktjJHMUew==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第37集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoCLt/iwARBoDON6E0nkOJYGAlcuhujbWMpXtRy30MoqE3NxMn0qyU4kvPqFFTHOTLQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第38集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoCLt/iwARBoDON6E0nkOJYGLpTX1c30rDMjLE35ALhZ+3kj7p3sPETzfdpwIYJcU/Q==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第39集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoKt58LyyIkbyCzghuCpPWR8j6gRC0RpK+QKiIvinTkUCL+sxnJucPbfidOpQbRJ8eg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第40集",
                    "playType": "Local",
                    "address": "igZI6MS2+qoWVXE8HqAuoKt58LyyIkbyCzghuCpPWR+wVSX6dU2ejCgaztbx9eLu6NvukwNJBAeOm8sz+XIU2Q==",
                    "action": "ServerGet",
                    "isIframePlay": false
                }
            ],
            "vipPlay": 0
        },
        {
            "name": "备用源2",
            "typeName": "BY2",
            "datalist": [
                {
                    "name": "第01集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H/HcKop7xw/ARsp1bKgVm8YmvA+/kIpV/MoCDl5D4VBzw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第02集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H/OXlqj8AKsrhaSFuIBWi76DId+LbVDru+gN+3OE6JJ6A==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第03集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H+zcy8p/f4moLYmm2TN8twSe0iyPdMyR6BGQhTKOZALyA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第04集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+wkL9DG34y7MuZC80GyFCDyxR4m7EwuTlqzuZm59FuJrwvgUXoOhB622MNg51S9Wl8=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第05集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+wbTd3I7ndrSIB0IyUgsaQ95GprtthY8BX+LxE5FM2aq0AzsDsYPoVNF5UseGfyNJQ=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第06集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+xZxy6uAu5QZpQzGSaAW3b7uBEtayyjsGoYyXnwmNXo5BODuk1On6/VxXjyxQCn+W8=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第07集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+x8NoeeXZJ/JMqc6Ko9y+SFaoGhA+I8Ri94OVSzADcoyjK1YFOjgl5UOuFa7KfYIgs=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第08集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+y+wDrK3WzYpiQEsbXLoPtjBm/aZdQy05UQnw+aDUghhPeuc1KzyOPswdvyqhHrbBM=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第09集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMRdNAQbxJGy+lBkF8U4VvgeyXLIuiIfb1oxpO8KNQz4VTGUg6MNpt+Gb1VW3d+5WVs=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第10集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMQiBrazShoiDQBhfatkrXprJTrg8jj0nuN4oTZPpSXDGUaKJXB5zwykmo1KoCPJlHs=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第11集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H/XCdbn/l1GAAU507XuJWlIGVX/4EebZ3ivYiuOxSDlzg==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第12集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H+f+Ff95UwJ4QFTpoEiIZHAUjyQcHMol71AcQfXrxCVQw==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第13集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMTF7MOC0MQKY9VlyV3mVQictRmjxBGGFbEJD+I3Tk3FWZCv5MqJ+mHb0eTXoPX4VlQ=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第14集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMTziwmq26YAsuq3d0A4LrHUjOKNz/0kbDxlccNyArNty9neenjqSrXj9kOkMuzBn1g=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第15集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMS49AIgFixbny4fBfL7Z3Jqif5YedoVh4Sbd1UdWqIhxNPBOUV72gB+bOHlCWUqfQ8=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第16集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMQGqWnJL1cJpGSjS48CODIPlSU73RPmQXcq10QUaA0Ujcg/l2H0qFeGDEOpB/ajOas=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第17集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMSvPAUIghVX39ZbH97+oZ3kU61rInHFuaaSXP71nbe1GqOd05jKIz59eYzGLKZHL54=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第18集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMSTHV3TkLGBsjjI4NQEhSYgRa5vjc4D56H1S+A9PTJYfOOjJxp2sfgkgPz4OMk9vV0=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第19集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMQH4zMyb94kEetOuEY9A5jC/DkwWL5OUk70l/zxNSc1YqLR5/Pr4/3OCrvuZeE6BZs=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第20集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMSOU8XEyAL+kxY9coLB1qxsqWh8SgOD+WNfqJU8EXy9manN2/7tn1096mC0wAPd68g=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第21集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMQ1VEV2jTE8M01mXhVc4drlUnLsyd8nZPQE73Nk1beph3Or9wOWgkik3okqZbGiC+0=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第22集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMS7G2Gxg7PFTbMqzLAbjuWYMxSrpDjAKVGWDtLAR+p3m4oyHAuuvUvf4noo0TcEghs=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第23集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+za31zFwNm3L+yWVyJHl+wOumc1uLzZQgWpcIr8N7TyEiOEo4E/URRAkkF7NsBZME0=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第24集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+yU8l6zjlzJ2K321lqzyEUDsAa5ONOLTzKEBkxCXl5l29fkhTZPBUBwH/dPlQi6X/E=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第25集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H88eh7mToyB6yPXCXvV8G325VNyeztCHoMKc0TmEhdLTDY7ND1ZdpLUqxHA6r0L6ew=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第26集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H/W22iJtGsJvMdQLE/A7ae/vTOzAXqRWYvzJVzlMRKRs66x2VMlRqER30SOZk9daYg=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第27集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H+hj+9t5o+SkdocIw5b6M5t+dFPal3RLG7QKxo4pnuvXgtbNGy0KndPHw2mW2am8Xc=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第28集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H9kEgGiy/BaF0x9ushmUyDv+NwtZ1hCw7f29crmFZ9hszdYs/+Xny1J3QhAigNpvR4=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第29集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H+GzcSiKOVuIaiSPUOqarm8SV1GJaht4/wCiqEojn0SKkKIOCeyXwf+d3XZY5Ac76s=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第30集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+z0siO8Ql4l868iyc4lD1lWvl5qzC3/Kdb+vveLnr1Be4ZHvf7Ac3uIE5T9tKULFe8=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第31集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H9uvUVIA89Wey+S40srYHmkqClay2mSGDiEtwf1yXGbhtZcuvOAfZMcVuoYBERB3ac=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第32集",
                    "playType": "Local",
                    "address": "vKvaufEg44ALz6oteGVVqngRS46KMRkuOHjI+ADK+H859fjcRXSYWGrY3IeY0g5E2YMPunA/ELMhKubPWdwet9H5MiIc1OdVQKNCndOC2Ig=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第33集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMSnkzmlagIXxSBM0imiXuh+o5sAhg8vtrkkJL+fyTU2fl2x8PdCDJrclKRnJJPyrvw=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第34集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMTMTuH2ymQ7bJQrp2xnF68XzWhxDpbYOzrCDE42xaosbphbKuoit+BKZ2Rcr0SrhjE=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第35集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+y3ws0fbRtHWuKr/WzWtFYLP6+eK4BGcuqQgJL3VZWu3WBNou4hYETi9Y26aIlevaw=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第36集",
                    "playType": "Local",
                    "address": "dSXgO37IOBF02UwwCxvFRyeR0g9x1pRvjLGhetW2t+yy18ZZLbrkkuWseYFAb7vkYGKDH8/o7AjtZHuxCNuo6iRDxGPDEZdpVkXGo01Hr+Y=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第37集",
                    "playType": "Local",
                    "address": "L1Upv91iWx+l4KpjOOXEHezgEBqIPUuw9j9j533UrMQoA+SzkEqs/U4M5syjKtgxKaJd9rGxSG+QWg6dxe5F7OAWhOhnlLu7o8p6S8oxRU8=",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第38集v2",
                    "playType": "Local",
                    "address": "d2Dj32lIRWL95PDpOUjNy0Ce3+dG+g6uKuUEW07vxV3ThVD22o2XFyd2fSf8sTwKY4by6WYX3q18j02RtaCmDA==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第39集",
                    "playType": "Local",
                    "address": "d2Dj32lIRWL95PDpOUjNy0Ce3+dG+g6uKuUEW07vxV0JgLqZbPVl+hXMH0OxUaNNIK7dndRQCs42Mjhr7pjjfQ==",
                    "action": "ServerGet",
                    "isIframePlay": false
                },
                {
                    "name": "第40集",
                    "playType": "Local",
                    "address": "d2Dj32lIRWL95PDpOUjNy0Ce3+dG+g6uKuUEW07vxV2Zj3Ih4jWBgFtw8WDc2N+80/eY3rqwPDWI99x6TkhE0w==",
                    "action": "ServerGet",
                    "isIframePlay": false
                }
            ],
            "vipPlay": 0
        }
    ],
    "modifyTime": 1688976273248,
    "expirationTime": 5000,
    "isMovie": false,
    "isHavePlayAd": false,
    "updateTime": false
}
```

## 3.3 点击查看此板块更多视频
> 【HttpGet】- {domain}api/v1.0/movie/getMoreList?category={分类参数}&type={板块参数}&pageIndex={起始页}&pageSize={页面数量}

``分类参数从``3.1描述``的参数中获取``    
``板块参数从``3.1获取的结果``中获取``  
``起始页默认从``1``开始``  
``页面数量默认从``21``开始``  
``页面总数和当前页数从``本接口``中获取``  
``可以做成懒加载列表``

返回数据：
```JSON
{
    "pageIndex": 1,
    "pageSize": 21,
    "totalPage": 1,
    "totalCount": 6,
    "items": [
        {
            "movieId": "63c2c5676a582a76ce01c90e",
            "pic": "https://file2.yqk99.app/image/movieCover/2023/04/12/18/14/8afe81f647b04662ac8c4ddcb0d2c0b3.jpg",
            "picJuHe": "",
            "name": "狂飙",
            "createTime": 1682180744949,
            "modifyTime": 1682180744949,
            "tvNumber": 39,
            "score": 8.5,
            "serializeStatus": 1,
            "category": "TVSeries",
            "updateTime": false
        },
        {
            "movieId": "646b4b1f9d6b3c4bf1b5a1c2",
            "pic": "https://file2.yqk99.app/image/movieCover/2023/05/26/16/26/63080a44641246c2819576a861a8a7df.jpg",
            "picJuHe": "",
            "name": "三分野",
            "createTime": 1686194594871,
            "modifyTime": 1686194594869,
            "tvNumber": 32,
            "score": 6.3,
            "category": "TVSeries",
            "updateTime": false
        },
        {
            "movieId": "64787ddb9d6b3c4bf1d7a965",
            "pic": "https://file2.yqk99.app/image/movieCover/2023/06/01/19/14/b2426a60694c4e0facbddbfdc235c1d5.jpg",
            "picJuHe": "",
            "name": "花戎",
            "createTime": 1686194582114,
            "modifyTime": 1686194582113,
            "tvNumber": 26,
            "serializeStatus": 0,
            "category": "TVSeries",
            "updateTime": false
        },
        {
            "movieId": "645a286e73f0ff5221561494",
            "pic": "https://file2.yqk99.app/image/movieCover/2023/05/10/16/51/0a1ead5d71c4416484bf55de544410fb.jpg",
            "picJuHe": "",
            "name": "护心",
            "createTime": 1686194558636,
            "modifyTime": 1686194558635,
            "tvNumber": 40,
            "score": 7.1,
            "serializeStatus": 1,
            "category": "TVSeries",
            "updateTime": false
        },
        {
            "movieId": "6460e7cd73f0ff5221692b45",
            "pic": "https://file2.yqk99.app/image/movieCover/2023/05/19/15/10/7057e0cbf8a74b1a948e30fd2f6334f4.jpg",
            "picJuHe": "",
            "name": "后浪",
            "createTime": 1686194543929,
            "modifyTime": 1686194543928,
            "tvNumber": 40,
            "serializeStatus": 1,
            "category": "TVSeries",
            "updateTime": false
        },
        {
            "movieId": "647894c59d6b3c4bf1d802f1",
            "pic": "https://file2.yqk99.app/image/movieCover/2023/06/07/16/02/671809fb664242e4898c1c264017e989.jpg",
            "picJuHe": "",
            "name": "梦中的那片海",
            "createTime": 1686194527219,
            "modifyTime": 1686194527217,
            "tvNumber": 24,
            "serializeStatus": 0,
            "category": "TVSeries",
            "updateTime": false
        }
    ]
}
```

## 3.4 猜你喜欢
> 【HttpGet】- {domain}api/v1.0/movie/getGuessWhatYouLike?movieId={视频id}&tvId={电视直播id？}&scheduleId={时间表id？}

``视频id可以从前面的任何一个列表数据接口中获取到``
这里的电视直播id，时间表id还不知道是什么，默认为空

返回数据：
```JSON
[
    {
        "movieId": "620fe5c1f0dcbc757f71e370",
        "pic": "https://pic.wujinimg.com/upload/vod/20211024-1/40184c1d77bf1ed6cc6abb7d9913a180.jpg",
        "name": "国家宝藏·展演",
        "tvNumber": 18,
        "sourceAddress": "WJZY||2021-10-23期$https://new.iskcd.com/20211024/C04xYZA1/index.m3u8#2021-10-24期$https://new.iskcd.com/20211025/0h7nOkHJ/index.m3u8#2021-10-30期$https://new.iskcd.com/20211031/DiyyZqVf/index.m3u8#2021-10-31期$https://new.iskcd.com/20211101/hLqGIdyK/index.m3u8#2021-11-06期$https://new.iskcd.com/20211107/PibFpIAO/index.m3u8#2021-11-07期$https://new.iskcd.com/20211108/FjdgxHzO/index.m3u8#2021-11-13期$https://new.iskcd.com/20211113/oAagSIct/index.m3u8#20211114期$https://new.iskcd.com/20211114/Io8TGQ2x/index.m3u8#20211120期$https://new.iskcd.com/20211120/CDHOmIeA/index.m3u8#20211121期$https://new.iskcd.com/20211121/25Q0xLdx/index.m3u8#20211127期$https://new.iskcd.com/20211127/7gOXPfYP/index.m3u8#20211128期$https://new.iskcd.com/20211128/YA4GiIpP/index.m3u8#20211204期$https://new.iskcd.com/20211204/HnyM8oE9/index.m3u8#20211205期$https://new.iskcd.com/20211205/RalRTVDO/index.m3u8#20211211期$https://new.iskcd.com/20211211/iA8bFkbt/index.m3u8#20211218期$https://new.iskcd.com/20211218/3fTrgrsw/index.m3u8#20211219期$https://new.iskcd.com/20211219/3DhmLf60/index.m3u8#20211225期$https://new.iskcd.com/20211225/sgJLswhS/index.m3u8",
        "modifyTime": 1645215058179,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e678",
        "pic": "http://ali-uget.static.yximgs.com/bs2/courseHead/753758722318268298",
        "name": "2022年安徽卫视春节联欢晚会",
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220201/NDIdtVIm/index.m3u8",
        "modifyTime": 1678271352176,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5bcf0dcbc757f71dc8d",
        "pic": "https://pic.wujinimg.com/upload/vod/20210814-1/f40f3de09e61aa48424e386ca8c31f31.png",
        "name": "芒果捞星闻",
        "tvNumber": 86,
        "sourceAddress": "WJZY||第20210813期$https://new.iskcd.com/20210814/miafPmkk/index.m3u8#第20210815期$https://new.iskcd.com/20210815/LWsEUS02/index.m3u8#第20210817期$https://new.iskcd.com/20210817/XrvFFI66/index.m3u8#第20210818期$https://new.iskcd.com/20210818/eRBSFG4B/index.m3u8#第20210819期$https://new.iskcd.com/20210819/pdpADzlL/index.m3u8#第20210820期$https://new.iskcd.com/20210820/hYjfQEHE/index.m3u8#第20210821期$https://new.iskcd.com/20210821/P35ObbDB/index.m3u8#第20210822期$https://new.iskcd.com/20210822/5HTvrCY5/index.m3u8#第20210823期$https://new.iskcd.com/20210823/ErhJPZP1/index.m3u8#第20210824期$https://new.iskcd.com/20210824/izNcoQEX/index.m3u8#第20210825期$https://new.iskcd.com/20210825/MidRDuoV/index.m3u8#第20210826期$https://new.iskcd.com/20210826/I3WLALBf/index.m3u8#第20210828期$https://new.iskcd.com/20210828/ECWuK0AD/index.m3u8#第20210830期$https://new.iskcd.com/20210830/4suGce3N/index.m3u8#第20210831期$https://new.iskcd.com/20210831/GqFxl55o/index.m3u8#第20210902期$https://new.iskcd.com/20210902/ajknGLZQ/index.m3u8#第20210903期$https://new.iskcd.com/20210903/CytSkOOx/index.m3u8#第20210904期$https://new.iskcd.com/20210904/HcpXooLU/index.m3u8#第20210905期$https://new.iskcd.com/20210905/vyy9se4N/index.m3u8#第20210906期$https://new.iskcd.com/20210906/QP01wOEz/index.m3u8#第20210907期$https://new.iskcd.com/20210907/4phPATUF/index.m3u8#第20210908期$https://new.iskcd.com/20210908/C3AaCgLC/index.m3u8#第20210909期$https://new.iskcd.com/20210909/qU60djdg/index.m3u8#第20210910期$https://new.iskcd.com/20210910/fe80wkeq/index.m3u8#第20210911期$https://new.iskcd.com/20210911/Q0IsIKLz/index.m3u8#第20210912期$https://new.iskcd.com/20210912/XimeaX9K/index.m3u8#第20210913期$https://new.iskcd.com/20210913/P5FetjA8/index.m3u8#第20210914期$https://new.iskcd.com/20210914/07BZRtAh/index.m3u8#第20210915期$https://new.iskcd.com/20210915/d5Av4dFY/index.m3u8#第20210916期$https://new.iskcd.com/20210916/jQu1HTgj/index.m3u8#第20210918期$https://new.iskcd.com/20210918/QTQklbxs/index.m3u8#第20210919期$https://new.iskcd.com/20210919/AeqTuN2a/index.m3u8#第20210920期$https://new.iskcd.com/20210920/Nk8YaEpj/index.m3u8#第20210921期$https://new.iskcd.com/20210921/at8Ww91w/index.m3u8#第20210922期$https://new.iskcd.com/20210922/UZaSk9pI/index.m3u8#第20210923期$https://new.iskcd.com/20210923/F6Qz0JfH/index.m3u8#第20210924期$https://new.iskcd.com/20210924/AWDuWxos/index.m3u8#第20210925期$https://new.iskcd.com/20210925/e5oouf7q/index.m3u8#第20210926期$https://new.iskcd.com/20210927/QUqvGaWw/index.m3u8#第20210927期$https://new.iskcd.com/20210927/Hq8giokr/index.m3u8#第20210928期$https://new.iskcd.com/20210928/D3Xxed0b/index.m3u8#第20210929期$https://new.iskcd.com/20210929/TiRA8mPE/index.m3u8#第20210930期$https://new.iskcd.com/20210930/aW00oIq1/index.m3u8#第20211001期$https://new.iskcd.com/20211001/zZuJOc8G/index.m3u8#第20211002期$https://new.iskcd.com/20211002/bb41squ8/index.m3u8#第20211003期$https://new.iskcd.com/20211003/iZNXA0UE/index.m3u8#第20211004期$https://new.iskcd.com/20211004/WagcM6aL/index.m3u8#第20211005期$https://new.iskcd.com/20211005/sEEoqyWK/index.m3u8#第20211006期$https://new.iskcd.com/20211006/QDfMpJON/index.m3u8#第20211007期$https://new.iskcd.com/20211007/GitWDPbc/index.m3u8#第20211008期$https://new.iskcd.com/20211008/58k77zVQ/index.m3u8#第20211009期$https://new.iskcd.com/20211009/5CX56Krl/index.m3u8#第20211010期$https://new.iskcd.com/20211010/hpnEQ7jl/index.m3u8#第20211012期$https://new.iskcd.com/20211012/RWIUcKM8/index.m3u8#第20211013期$https://new.iskcd.com/20211013/cGI9H5dV/index.m3u8#第20211014期$https://new.iskcd.com/20211014/7Jn2iSfU/index.m3u8#第20211015期$https://new.iskcd.com/20211015/M1kO4gBz/index.m3u8#第20211016期$https://new.iskcd.com/20211016/1hjTpqOe/index.m3u8#第20211017期$https://new.iskcd.com/20211017/5jhYyLKN/index.m3u8#第20211018期$https://new.iskcd.com/20211018/WzbZ9zO1/index.m3u8#第20211019期$https://new.iskcd.com/20211019/ZMY55SlP/index.m3u8#第20211020期$https://new.iskcd.com/20211020/ZJEs1wYN/index.m3u8#第20211021期$https://new.iskcd.com/20211021/h1BP58Yf/index.m3u8#第20211022期$https://new.iskcd.com/20211022/KjtsHEPm/index.m3u8#第20211023期$https://new.iskcd.com/20211023/g7sdDhIo/index.m3u8#第20211024期$https://new.iskcd.com/20211024/OJGgDOYi/index.m3u8#第20211025期$https://new.iskcd.com/20211025/5xVA1fsp/index.m3u8#第20211026期$https://new.iskcd.com/20211026/9rAirovN/index.m3u8#第20211027期$https://new.iskcd.com/20211027/dGJe6Ym2/index.m3u8#第20211028期$https://new.iskcd.com/20211028/JIMonMqC/index.m3u8#第20211029期$https://new.iskcd.com/20211029/gfhvUWpu/index.m3u8#第20211030期$https://new.iskcd.com/20211030/bM72D2vx/index.m3u8#第20211031期$https://new.iskcd.com/20211031/SUyXoy4e/index.m3u8#第20211101期$https://new.iskcd.com/20211101/pjthXTOc/index.m3u8#第20211102期$https://new.iskcd.com/20211102/ztTMhFEE/index.m3u8#第20211103期$https://new.iskcd.com/20211103/6mU5tFuD/index.m3u8#第20211104期$https://new.iskcd.com/20211104/7tnYycOL/index.m3u8#第20211105期$https://new.iskcd.com/20211105/yzFsWcWx/index.m3u8#第20211106期$https://new.iskcd.com/20211106/jzb4uHgz/index.m3u8#第20211107期$https://new.iskcd.com/20211107/gML8vsTp/index.m3u8#第20211108期$https://new.iskcd.com/20211108/t0bmC1N7/index.m3u8#第20211109期$https://new.iskcd.com/20211109/Q7DiCqFB/index.m3u8#第20211110期$https://new.iskcd.com/20211110/UxNjGdHw/index.m3u8#第20211111期$https://new.iskcd.com/20211111/pw852sk2/index.m3u8#第20211112期$https://new.iskcd.com/20211112/AMRtjJrE/index.m3u8#第20211113期$https://new.iskcd.com/20211113/N3t3BhpH/index.m3u8",
        "modifyTime": 1645215054028,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e66c",
        "pic": "http://ali-uget.static.yximgs.com/bs2/courseHead/3348520670396251216",
        "name": "2022年辽宁卫视春节联欢晚会",
        "score": 6.7,
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220131/MZ82UPQg/index.m3u8$$$BY1||HD国语$https://vip.ffzy-online2.com/20230214/14028_9fd5967c/index.m3u8",
        "modifyTime": 1678271352184,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e66d",
        "pic": "https://pic.wujinimg.com/upload/vod/20220131-1/837b05ebeb6c74b53dffacbe899ec0c2.jpg",
        "name": "2022年天津卫视德云社相声春晚\u200e",
        "score": 6.6,
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220131/7tQnEDSx/index.m3u8",
        "modifyTime": 1678271352182,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e679",
        "pic": "https://pic.wujinimg.com/upload/vod/20220201-1/c04156cbddba85fd9886066e76465ff4.jpg",
        "name": "2022年山东卫视春节联欢晚会",
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220201/m3NfpeHq/index.m3u8",
        "modifyTime": 1678271352175,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e67a",
        "pic": "http://ali-uget.static.yximgs.com/bs2/courseHead/7466996222079184154",
        "name": "2022年吉林卫视春节联欢晚会",
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220201/LITSB47I/index.m3u8",
        "modifyTime": 1678271352173,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e67d",
        "pic": "https://pic.wujinimg.com/upload/vod/20220201-1/8a6bc0387a753dfdbacf999ccdef8e4a.jpg",
        "name": "2022年河北网络春晚",
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220201/gOkaMdZh/index.m3u8",
        "modifyTime": 1678271352170,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e693",
        "pic": "https://pic.wujinimg.com/upload/vod/20220202-1/8eb6a2a0f184c74e7047455197837cbb.jpg",
        "name": "春满东方点亮幸福·2022东方卫视春节晚会",
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220202/NusCv5vb/index.m3u8",
        "modifyTime": 1678271344237,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e6c0",
        "pic": "https://pic.wujinimg.com/upload/vod/20220205-1/4c28f81109efb67a1dc213acf3dea3e8.jpg",
        "name": "2022年冬奥会开幕式",
        "tvNumber": 1,
        "sourceAddress": "WJZY||HD$https://new.qqaku.com/20220205/r8f0RzfL/index.m3u8",
        "modifyTime": 1678271336290,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "6216241896a1f9512c5d9c81",
        "pic": "https://pic.wujinimg.com/upload/vod/20220218-1/495d28bd92aef7acac2a8b789dfdd125.jpg",
        "name": "名侦探学院第五季",
        "score": 8.3,
        "tvNumber": 44,
        "sourceAddress": "WJZY||20211125$https://new.qqaku.com/20211125/9UJtRlK3/index.m3u8#20211126$https://new.qqaku.com/20211126/ogTbG8ds/index.m3u8#20211202$https://new.qqaku.com/20211202/Oda6VGiO/index.m3u8#20211203$https://new.qqaku.com/20211204/vfQc53fD/index.m3u8#20211205$https://new.qqaku.com/20211206/ZgSrktpW/index.m3u8#20211208$https://new.qqaku.com/20211208/v28Z2cEj/index.m3u8#20211209$https://new.qqaku.com/20211209/54reXuSx/index.m3u8#20211210$https://new.qqaku.com/20211210/gevXssJ6/index.m3u8#20211215$https://new.qqaku.com/20211215/wILZNkMh/index.m3u8#20211216$https://new.qqaku.com/20211216/mN0QWBXe/index.m3u8#20211222$https://new.qqaku.com/20211222/HWnWWrZt/index.m3u8#20211223$https://new.qqaku.com/20211223/8zBxyD4c/index.m3u8#20211224$https://new.qqaku.com/20211224/oi03Rg14/index.m3u8#20211226$https://new.qqaku.com/20211228/RYYcl8Bi/index.m3u8#20211229$https://new.qqaku.com/20211229/Z59qTXm0/index.m3u8#20211230$https://new.qqaku.com/20211230/cY03Bk2W/index.m3u8#20220105$https://new.qqaku.com/20220105/aGb1sd47/index.m3u8#20220106$https://new.qqaku.com/20220106/ItCM8n1P/index.m3u8#20220107$https://new.qqaku.com/20220108/oNQBN2H9/index.m3u8#20220112$https://new.qqaku.com/20220112/4kiL9y2n/index.m3u8#20220113$https://new.qqaku.com/20220113/5sPKooC4/index.m3u8#20220115$https://new.qqaku.com/20220115/CCzr0YQh/index.m3u8#20220118$https://new.qqaku.com/20220118/YPqepjws/index.m3u8#20220119$https://new.qqaku.com/20220119/XONeYCHF/index.m3u8#20220120$https://new.qqaku.com/20220120/aIRmXpvb/index.m3u8#20220121$https://new.qqaku.com/20220121/yEpILCZQ/index.m3u8#20220122$https://new.qqaku.com/20220122/Dsae6nY3/index.m3u8#20220125$https://new.qqaku.com/20220125/Y0q6YSsh/index.m3u8#20220126$https://new.qqaku.com/20220126/TOw6Vem3/index.m3u8#20220127$https://new.qqaku.com/20220127/PeNSVFlr/index.m3u8#20220129$https://new.qqaku.com/20220129/kQJlhfJ7/index.m3u8#20220202$https://new.qqaku.com/20220202/CtBJrlxB/index.m3u8#20220203$https://new.qqaku.com/20220203/Z5F7V0ku/index.m3u8#20220204$https://new.qqaku.com/20220204/9kkXu8xU/index.m3u8#20220205$https://new.qqaku.com/20220205/MhMPO7dz/index.m3u8#20220208$https://new.qqaku.com/20220208/oUhLbBpg/index.m3u8#20220210$https://new.qqaku.com/20220210/Tzcc7hlM/index.m3u8#20220212$https://new.qqaku.com/20220212/7XL5wNhL/index.m3u8#20220216$https://new.qqaku.com/20220216/xtTwvyOs/index.m3u8#20220217$https://new.qqaku.com/20220217/S9td1Rjx/index.m3u8#20220218$https://new.qqaku.com/20220218/vTaLBFT8/index.m3u8#20220219$https://new.qqaku.com/20220219/TYHia8R7/index.m3u8#20220222$https://new.qqaku.com/20220223/KM8PENZh/index.m3u8#20220225$https://new.qqaku.com/20220225/Dru1uXIG/index.m3u8",
        "modifyTime": 1678271087326,
        "category": "VarietyShow",
        "movie": false
    },
    {
        "movieId": "620fe5c2f0dcbc757f71e59f",
        "pic": "https://pic.wujinimg.com/upload/vod/20220122-1/13635c9ba0506255a5dfb9e8849da3d6.jpg",
        "name": "新郎课程",
        "tvNumber": 1,
        "sourceAddress": "WJZY||20220119$https://new.qqaku.com/20220122/uhxKBjJj/index.m3u8",
        "modifyTime": 1678271376557,
        "category": "VarietyShow",
        "movie": false
    }
]
```

## 3.5 热词
> 【HttpGet】- {domain}api/v1.0/hotSearch/get
获取热搜词，几个类别

```JSON
[
    {
        "title": "热搜",
        "list": [
            {
                "name": "长风渡"
            },
            {
                "name": "我的人间烟火"
            },
            {
                "name": "安乐传"
            },
            {
                "name": "消失的十一层"
            },
            {
                "name": "偷偷藏不住"
            },
            {
                "name": "玉骨遥"
            },
            {
                "name": "追光的日子"
            },
            {
                "name": "曾少年"
            },
            {
                "name": "消失的她"
            },
            {
                "name": "八角笼中"
            }
        ]
    },
    {
        "title": "电影",
        "list": [
            {
                "name": "变形金刚：超能勇士崛起"
            },
            {
                "name": "瞬息全宇宙"
            },
            {
                "name": "我爱你！"
            },
            {
                "name": "东北警察故事2"
            },
            {
                "name": "犯罪都市3"
            },
            {
                "name": "速度与激情10 Fast X"
            },
            {
                "name": "夺宝奇兵5：命运转盘"
            },
            {
                "name": "蜘蛛侠：纵横宇宙"
            },
            {
                "name": "战斧行动2喋血"
            },
            {
                "name": "枪神再起"
            }
        ]
    },
    {
        "title": "电视剧",
        "list": [
            {
                "name": "繁华似锦"
            },
            {
                "name": "雪鹰领主2023"
            },
            {
                "name": "狂飙"
            },
            {
                "name": "她只是不想输（闪耀的她）"
            },
            {
                "name": "当我飞奔向你"
            },
            {
                "name": "南洋女儿情"
            },
            {
                "name": "古相思曲"
            },
            {
                "name": "公诉"
            },
            {
                "name": "后宫·甄嬛传"
            },
            {
                "name": "爱的勘探法"
            }
        ]
    },
    {
        "title": "综艺",
        "list": [
            {
                "name": "密室大逃脱 第五季"
            },
            {
                "name": "奔跑吧 第七季"
            },
            {
                "name": "向往的生活 第七季"
            },
            {
                "name": "乘风2023"
            },
            {
                "name": "萌探探探案 第三季"
            },
            {
                "name": "爸爸当家第二季"
            },
            {
                "name": "五十公里桃花坞3"
            },
            {
                "name": "女子推理社"
            },
            {
                "name": "全员加速中 第三季"
            },
            {
                "name": "一拍即合的我们"
            }
        ]
    },
    {
        "title": "动漫",
        "list": [
            {
                "name": "完美世界"
            },
            {
                "name": "斗罗大陆"
            },
            {
                "name": "神印王座"
            },
            {
                "name": "喜羊羊与灰太狼之遨游神秘洋"
            },
            {
                "name": "斗破苍穹 年番"
            },
            {
                "name": "咒术回战 第二季"
            },
            {
                "name": "沧元图"
            },
            {
                "name": "海贼王"
            },
            {
                "name": "斗罗大陆2：绝世唐门"
            },
            {
                "name": "吞噬星空"
            }
        ]
    }
]
```

## 3.6 关键词搜索
> 【HttpGet】- {domain}api/v1.0/search/getMovieByOptions?q={关键词}&category={分类}&type={类型}&area={地区}&year={年代}&sort={排序}&moviePlayType={播放类型}&pageIndex={页面下标}&pageSize={每页item数量}

<font color="red">关键词搜索、筛选都是使用这个接口！</font>
- 关键词搜索举例：  
  - 关键词=机智  
  - 分类=-1
  - 类型=-1
  - 地区=-1
  - 年代=-1
  - 排序=-1
  - 播放类型=-1
  - 播放页面下表=1
  - 每页item数量=21

```JSON
{
    "pageIndex": 1,
    "pageSize": 21,
    "totalPage": 1,
    "totalCount": 19,
    "items": [
        {
            "movieId": "610d809258c30b019f1cc28f",
            "pic": "https://pic.rmb.bdstatic.com/bjh/03ea1b1bfdcb020a3b3ed13d80d0d477.jpeg",
            "name": "机智过人",
            "category": "综艺",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_QiYi",
            "director": "高博",
            "actress": "高博",
            "year": "2017",
            "area": "China",
            "heat": 0,
            "releaseTime": 1514476800000,
            "isFavorite": false,
            "tvNumber": 12,
            "modifyTime": 1655110873844,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "626a4b37d779d35f3eda29a0",
            "pic": "https://puui.qpic.cn/vcover_vt_pic/0/l9yjoz2clui1og51548922796/0",
            "name": "机智的山羊",
            "category": "动漫",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_Tencent",
            "director": "万超尘",
            "year": "1956",
            "area": "China",
            "heat": 0,
            "releaseTime": -428745600000,
            "isFavorite": false,
            "tvNumber": 1,
            "modifyTime": 1651133239671,
            "movieCategory": "Anime",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "614c96d0104f701ea6bebed0",
            "pic": "http://pic7.iqiyipic.com/image/20210922/90/32/a_100459354_m_601_m6_260_360.jpg",
            "name": "机智的恋爱",
            "category": "综艺",
            "starLevel": 0,
            "score": 5,
            "sourceAddress": "BaiDuJx_QiYi",
            "actress": "蔡康永,李沁,魏晨,段星星,大王,刘人语,",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1622476800000,
            "isFavorite": false,
            "tvNumber": 20,
            "modifyTime": 1678275894666,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "620fe5b4f0dcbc757f71cf6b",
            "pic": "https://img2.doubanio.com/view/photo/s_ratio_poster/public/p2670009771.jpg",
            "name": "机智的上半场",
            "category": "电视剧",
            "starLevel": 0,
            "score": 6.6,
            "sourceAddress": "VIP_Source",
            "director": "黎志",
            "actress": "沈月,章若楠,梁靖康,薇薇,张歆怡,翟子路,张志浩,李佳航,丁冠森,刘钧,倪虹洁,敖瑞鹏,王以纶,刘洁",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1627747200000,
            "isFavorite": false,
            "tvNumber": 24,
            "serializeStatus": 1,
            "modifyTime": 1678274034416,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "620fe5b8f0dcbc757f71d5d3",
            "pic": "https://file2.yqk99.app/image/movieCover/2022/04/08/11/25/8ce5e4a5ec9447bf82b1a1cfc5a662c9.jpg",
            "name": "机智牢房生活",
            "category": "电视剧",
            "starLevel": 0,
            "score": 9.4,
            "sourceAddress": "VIP_Source",
            "director": "申元浩",
            "actress": "朴海秀,郑敬淏,郑秀晶,林华映,丁海寅,郑雄仁,金圣喆,李奎炯,郑敏圣,朴浩山,姜昇润,金景南,崔武成,崔胜元,成东日,金汉钟,周锡泰,姜基栋,朴亨洙,李道谦,申在河,廉惠兰,林哲亨,芮秀贞,李浩哲,刘在明,申隣雅,崔光一,张赫镇,金基楠,李太善,徐智勋",
            "year": "2017",
            "area": "Korea",
            "heat": 0,
            "releaseTime": 1509465600000,
            "isFavorite": false,
            "tvNumber": 16,
            "serializeStatus": 1,
            "modifyTime": 1678261906157,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "626a4b71d779d35f3edab4db",
            "pic": "http://i.gtimg.cn/qqlive/images/newcolumn/v1/v/vy9kmz.jpg",
            "name": "机智过人 第二季",
            "category": "综艺",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_Tencent",
            "actress": "朱广权,",
            "year": "2018",
            "area": "China",
            "heat": 0,
            "releaseTime": 1533916800000,
            "isFavorite": false,
            "tvNumber": 10,
            "modifyTime": 1657627288850,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "610d805d58c30b019f1cae39",
            "pic": "https://pic1.iqiyipic.com/image/20200916/bd/bb/a_100168948_m_601_m11_260_360.jpg",
            "name": "机智过人第二季",
            "category": "综艺",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_QiYi",
            "director": "朱广权",
            "actress": "朱广权,韩雪,吴秀波,赵立新,明道,",
            "year": "2018",
            "area": "China",
            "heat": 0,
            "releaseTime": 1546099200000,
            "isFavorite": false,
            "tvNumber": 9,
            "modifyTime": 1651133157604,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "626a4b73d779d35f3edab71f",
            "pic": "http://puui.qpic.cn/vcolumn_vt_pic/0/ecu4cz1564212194/0",
            "name": "机智过人 第三季",
            "category": "综艺",
            "starLevel": 0,
            "sourceAddress": "General",
            "director": "朱广权",
            "actress": "朱广权,撒贝宁,韩雪,吴克群,",
            "year": "2019",
            "area": "China",
            "heat": 0,
            "releaseTime": 1561910400000,
            "isFavorite": false,
            "tvNumber": 11,
            "modifyTime": 1657616483058,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "614c96d0104f701ea6bebdfc",
            "pic": "https://pic7.iqiyipic.com/image/20210922/90/32/a_100459354_m_601_m6_260_360.jpg",
            "name": "丸美机智的恋爱",
            "category": "综艺",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_QiYi",
            "director": "",
            "actress": "蔡康永,李沁,魏晨,段星星,大王,刘人语,",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1632153600000,
            "isFavorite": false,
            "tvNumber": 2,
            "modifyTime": 1632987402578,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "610d8e2258c30b019f1ef135",
            "pic": "https://img9.doubanio.com/view/photo/s_ratio_poster/public/p2623285621.jpg",
            "name": "机智的恋爱生活",
            "category": "电视剧",
            "starLevel": 0,
            "score": 5.9,
            "sourceAddress": "BaiDuJx_Mango",
            "director": "李雁倩",
            "actress": "季肖冰,金沫汐,夏宁骏",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1622476800000,
            "isFavorite": false,
            "tvNumber": 33,
            "modifyTime": 1678270422888,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "620fe571f0dcbc757f715a72",
            "pic": "https://file2.yqk99.app/image/movieCover/2022/09/06/12/09/5c5629fad1f8411bb914b6e53c12166b.gif",
            "name": "机智的山村生活",
            "category": "综艺",
            "starLevel": 0,
            "score": 9.4,
            "sourceAddress": "VIP_Source",
            "director": "罗䁐锡",
            "actress": "曹政奭,柳演锡,郑敬淏,金大明,田美都",
            "year": "2021",
            "area": "Korea",
            "heat": 0,
            "releaseTime": 1633017600000,
            "isFavorite": false,
            "tvNumber": 9,
            "serializeStatus": 1,
            "modifyTime": 1678275972726,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "610d809858c30b019f1cc473",
            "pic": "https://pic2.iqiyipic.com/image/20201013/b5/32/a_100332557_m_601_m7_260_360.jpg",
            "name": "机智过人第三季",
            "category": "综艺",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_QiYi",
            "director": "朱广权",
            "actress": "朱广权,韩雪,魏晨,",
            "year": "2020",
            "area": "China",
            "heat": 0,
            "releaseTime": 1580313600000,
            "isFavorite": false,
            "tvNumber": 10,
            "modifyTime": 1651133160479,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "624ff28872c6a304b9197281",
            "pic": "http://sd-pic.com/upload/vod/20220408-1/52685503718937250df3d4269ab086f3.png",
            "name": "机智女法医港版粤语",
            "category": "电视剧",
            "starLevel": 0,
            "score": 7.9,
            "sourceAddress": "SDZY",
            "director": "楼健",
            "actress": "苏晓彤,王子奇,杨廷东,赵尧珂,王彦鑫,郭秋成,穆怀虎",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1622476800000,
            "isFavorite": false,
            "tvNumber": 17,
            "modifyTime": 1651139990897,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "61ea5b4ebda1f2632601abb6",
            "pic": "https://file2.yqk99.app/image/movieCover/2022/01/21/15/05/bf5103fb53ae40668b1fc683a8ac644f.jpg",
            "name": "机智医生生活 第二季",
            "category": "电视剧",
            "starLevel": 0,
            "score": 9.5,
            "sourceAddress": "VIP_Source",
            "director": "申元浩",
            "actress": "曹政奭,柳演锡,郑敬淏,金大明,田美都,河允庆",
            "year": "2021",
            "area": "Korea",
            "heat": 0,
            "releaseTime": 1622476800000,
            "isFavorite": true,
            "tvNumber": 12,
            "serializeStatus": 1,
            "modifyTime": 1678261906153,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "6135e62d000eb6237901d942",
            "pic": "https://img9.doubanio.com/view/photo/s_ratio_poster/public/p2586800409.jpg",
            "name": "机智医生生活 第一季",
            "category": "电视剧",
            "starLevel": 0,
            "score": 9.5,
            "sourceAddress": "General",
            "director": "申源浩",
            "actress": "曹政奭,柳演锡,郑敬淏,金大明,田美都,金海淑,金甲洙,丁文晟,申贤彬,文泰佑,金俊翰,河允庆,全光真,安恩真,金惠仁,崔英俊,申度贤,徐镇元,赵胜延,金俊,,郭善英,奇恩世,曹怡贤,裴贤圣,金秀珍,李露雅,李达,杨照雅,朴韩率,李慧恩,金妃妃,李智媛,尹惠利,金智星,薛有珍,成东日,金成钧,艺智苑,吴允儿,朴亨洙,黄英熙,朴胜泰,廉惠兰,金圣喆,郑宰成,李智勋,崔英佑",
            "year": "2020",
            "area": "Korea",
            "heat": 0,
            "releaseTime": 1582992000000,
            "isFavorite": false,
            "tvNumber": 12,
            "serializeStatus": 1,
            "modifyTime": 1657855548369,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "617d81310d05ff114b55cbfd",
            "pic": "http://3img.hitv.com/preview/sp_images/2021/09/26/202109260940459399606.jpg",
            "name": "机智的恋爱生活越南语版",
            "category": "电视剧",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_Mango",
            "director": "李雁倩",
            "actress": "季肖冰,金沫汐",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1621958400000,
            "isFavorite": false,
            "tvNumber": 32,
            "modifyTime": 1635615025945,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "617d81320d05ff114b55cc6b",
            "pic": "https://3img.hitv.com/preview/sp_images/2021/09/26/202109260940459399606.jpg",
            "name": "机智的恋爱生活 越南语版",
            "category": "电视剧",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_Mango",
            "director": "李雁倩",
            "actress": "季肖冰,金沫汐",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1621958400000,
            "isFavorite": false,
            "tvNumber": 32,
            "modifyTime": 1639094556636,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "616a164f104f701ea6cc420c",
            "pic": "https://2img.hitv.com/preview/sp_images/2021/05/28/202105281500236606105.jpg",
            "name": "机智的恋爱生活 “狐系”恋爱手册",
            "category": "电视剧",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_Mango",
            "director": "李雁倩",
            "actress": "季肖冰,金沫汐",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1622476800000,
            "isFavorite": false,
            "tvNumber": 15,
            "modifyTime": 1639094515175,
            "movieCategory": "TVSeries",
            "isIframePlay": false,
            "updateTime": false
        },
        {
            "movieId": "6189f2170d05ff114b574bb5",
            "pic": "https://pic0.iqiyipic.com/image/20211104/c1/c1/a_100483705_m_601_260_360.jpg",
            "name": "机智的恋爱——甜甜的恋爱，理性地谈",
            "category": "综艺",
            "starLevel": 0,
            "sourceAddress": "BaiDuJx_QiYi",
            "year": "2021",
            "area": "China",
            "heat": 0,
            "releaseTime": 1637164800000,
            "isFavorite": false,
            "tvNumber": 11,
            "modifyTime": 1639094557156,
            "movieCategory": "VarietyShow",
            "isIframePlay": false,
            "updateTime": false
        }
    ]
}
```
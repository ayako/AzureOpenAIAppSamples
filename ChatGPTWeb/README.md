# Azure OpenAI Service ChatGPT Sample Code (as of 2023/04/11)

This is chatbot app sample code using Azure OpenAI Service ChatGPT, JQuery and Node.js.

Azure OpenAI Service ChatGPT を利用した、チャットボットアプリのサンプルコードです。JQuery と Node.js を使用しています。


## 利用方法

English follows -> See [How to use](#how-to-use)

### 準備

- お持ちの Azure Subscription で Azure OpenAI Service の利用を申請＆許諾を得たのち、Azure OpenAI Service を作成し、API Key と Endpoint を取得しておきます。

### Azure OpenAI Service 情報の設定

- Azure OpenAI Service 作成時に取得した情報を public/index.js に設定します (1-2行目)。
- 本番利用では ソースコード内にこのような情報を **直接記載しないでください** 。こちらのコードは検証|ハンズオン用です。

### Node.js 環境設定 & アプリへのアクセス方法

- ターミナルを起動し以下のコマンドで必要なライブラリーをインストールします。

```
npm i
```

- ターミナルから以下のコマンドでサーバーを立ち上げ、ブラウザから [http://localhost:3000](http://localhost:3000) にアクセスしてアプリを表示します。

```
node server.js
```



## How to use

### Prep

- Apply and get approved Azure OpenAI Service acccess with your Azure Subscription, and create your Azure OpenAI Service, and get API Key and Endpoint.

### Settings Azure OpenAI Service in code

- Embed your Azure OpenAI Service info into public/index.js (line 1-2).
- **DO NOT** embed those info direct into source code for production use. This is only testing|hands-on purpose.

### Node.js Environment & Access to App

- Install libraries using following command from terminal.

```
npm i
```

- run local server using following command from terminal, access [http://localhost:3000](http://localhost:3000) from browser, and access to app.

```
node server.js
```

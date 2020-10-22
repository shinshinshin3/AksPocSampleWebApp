## 事前準備

### マネージドIDの作成  
https://portal.azure.com/#create/Microsoft.ManagedIdentity  
から手で作成(todo: CLIからの作成)

### アプリケーションゲートウェイの作成
https://portal.azure.com/#blade/HubsExtension/BrowseResourceBlade/resourceType/Microsoft.Network%2FapplicationGateways  
から手で作成(todo: CLIからの作成)

---
## k8s セットアップ

### namespaceの作成

~~~
# kubectl create namespace poc-application-appgw
~~~

### helmのインストール

~~~
# curl -fsSL -o get_helm.sh https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3
# chmod 700 get_helm.sh
# ./get_helm.sh
# helm repo add stable https://kubernetes-charts.storage.googleapis.com/

# helm repo add application-gateway-kubernetes-ingress https://appgwingress.blob.core.windows.net/ingress-azure-helm-package/
# helm repo update
~~~


## アプリケーションゲートウェイのIngress controllerのインストール
https://docs.microsoft.com/ja-jp/azure/application-gateway/ingress-controller-install-new

### rbac用のアカウント等の作成
~~~
# kubectl create -f https://raw.githubusercontent.com/Azure/aad-pod-identity/master/deploy/infra/deployment-rbac.yaml
~~~

### ingress controller のパラメータファイル準備
todo: 作成方法を定量的に書く
~~~
# curl https://raw.githubusercontent.com/Azure/application-gateway-kubernetes-ingress/master/docs/examples/sample-helm-config.yaml > helm-config.yaml

権限設定用のIDを入手
# az ad sp create-for-rbac --sdk-auth | base64 -w0

# vi helm-config.yaml
変更点
追加
appgw.subscriptionId: <サブスクリプションID>
appgw.name: aks-poc-appgw
armAuth.type: servicePrincipal
armAuth.secretJSON: <上のコマンドで入手したID>
rbac.enabled: true

削除
armAuth.identityResourceID
armAuth.identityClientID
~~~

### helmでingress controllerをインストール

~~~
# helm upgrade --install ingress-azure --version 1.2.0 -f helm-config.yaml application-gateway-kubernetes-ingress/ingress-azure
~~~

### secretsの作成

~~~
# kubectl -n poc-application-appgw create secret generic aks-poc-secret \
        --from-literal=dbConnectionString='<your db connection string>' \
        --from-literal=ApplicationInsights_InstrumentationKey=<your appinsights key>
~~~

### appの作成

~~~
# kubectl apply -n poc-application-appgw -f appgw/aks-poc-deployment-app.yaml
# kubectl apply -n poc-application-appgw -f appgw/aks-poc-ingress-appgw.yaml
~~~

### 外部IPの確認

~~~
# kubectl -n poc-application-appgw get ingress
NAME              HOSTS   ADDRESS          PORTS   AGE
aks-poc-ingress   *       20.194.179.223   80      4h10m
~~~
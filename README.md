# AksPocSampleWebApp

<pre>
$ docker-compose up -d --build
$ docker-compose up -d
$ curl -i "http://localhost/api/todoItems" -X POST -d '{"Name": "study", "IsComplete": true}' -H "Content-Type: application/json"
HTTP/1.1 201 Created
Server: nginx/1.19.3
Date: Thu, 15 Oct 2020 17:37:11 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Location: http://localhost/api/TodoItems/3
Request-Context: appId=cid-v1:26fc99a5-244d-4c60-a8ef-49cfc12b4a5f
</pre>


Ngnixのログ
<pre>
{"time": "2020-10-15T16:51:14+00:00","remote_addr": "172.22.0.1","request": "POST /api/todoItems HTTP/1.1","request_method": "POST","request_length": "176","request_uri": "/api/todoItems","uri": "/api/todoItems","query_string": "-","status": "201","bytes_sent": "346","body_bytes_sent": "52","referer": "-","user_agent": "curl/7.54.0","x_forward_for": "-","request_time": "0.056","upstream_response_time": "0.056"}
</pre>

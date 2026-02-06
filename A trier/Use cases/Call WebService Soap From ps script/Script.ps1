$svc = New-WebServiceProxy -Uri "http://urlForWsl?wsdl" 
$svc.Timeout = 30000000
$svc.CallMethod()
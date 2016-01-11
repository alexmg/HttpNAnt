HttpNAnt
========

Introduction
------------

A custom NAnt task that allows HTTP requests to be made to a REST web service from a NAnt script. The task supports all the HTTP methods and allows you to specify the content type and the content itself. You can also retrieve the response content and status code through properties set by the task.

Using the task
--------------

Using the task is simple. The only mandatory attribute is url and the default HTTP method is GET. Here is a sample NAnt project showing how to use the <http/> task.

```xml
<?xml version="1.0"?>
<project name="Http">
	<http url="http://httpbin.org/get"
        method="GET"
        contenttype="application/json"
        connectiontimeout="30"
        responseproperty="response"
        statuscodeproperty="status"
        failonerror="true" 
        username="jdoe"
        password="53cr34" />

  <echo message="Response: ${response}" />
  <echo message="Status Code: ${status}" />
</project>
```

You can provide content when using the POST method via the `content` attribute.

```xml
<?xml version="1.0"?>
<project name="Http">
	<http url="http://httpbin.org/post"
        method="POST"
        contenttype="application/json"
        connectiontimeout="30"
        responseproperty="response"
        statuscodeproperty="status"
        failonerror="true" 
        username="jdoe"
        password="53cr34"
        content="{ &quot;greeting&quot; : &quot;Hello world!&quot; }" />

  <echo message="Response: ${response}" />
  <echo message="Status Code: ${status}" />
</project>
```

Authentication, if enabled via the `username` and `password` attributes, uses Basic Authentication.

Building the solution
---------------------

After pulling down the source build the solution with Visual Studio or by running `msbuild Source\HttpNAnt.sln` from the command line. The `Dist` folder will contain the `AlexMG.HttpNAnt.dll` assembly. The compiled assembly has the `Microsoft.Http.dll` assembly from the WCF REST Starter Kit merged into it using the [ILMerge](http://www.microsoft.com/downloads/details.aspx?FamilyID=22914587-b4ad-4eae-87cf-b14ae6a939b0&DisplayLang=en) tool. This makes deployment easier by removing the chance of accidentally forgetting to deploy the dependency.

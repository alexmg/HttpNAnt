HttpNAnt
========

Introduction
------------

A custom NAnt task that allows HTTP requests to be made to a REST web service from a NAnt script. The task supports all the HTTP methods and allows you to specify the content type and the content itself. You can also retrieve the response content and status code through properties set by the task.

Using the task
--------------

Using the task is simple. The only mandatory attribute is url and the default HTTP method is GET. Here is a sample NAnt project showing how to use the <http/> task.

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
	        password="53cr34"
	        content="{ &quot;greeting}&quot; : &quot;Hello world!&quot; }" />
	
	  <echo message="Response: ${response}" />
	  <echo message="Status Code: ${status}" />
	</project>

Authentication, if enabled, uses Basic authentication.

Building the solution
---------------------

A post build event in the Visual Studio solution uses the ILMerge tool to merge the _Microsoft.Http.dll_ assembly (part of the WCF REST Starter Kit) into the output assembly _AlexMG.NAntTasks.dll_. This makes deployment easier by removing the chance of accidentally forgetting to deploy the _Microsoft.Http.dll_ dependency. You will also need to update the _xcopy_ command in the post-build event to point to the location of your NAnt bin folder.
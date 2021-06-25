// Powered by Infostretch 

pipeline {
    agent any
    
    stages{
	    stage ('Build') { 
 			// Shell build step
 			steps{
                sh """ 
                dotnet build nbaData.sln
                dotnet test --logger:"nunit;LogFileName=TestResults.xml" nbaData.sln 
                """
 			}
 			
	        post {
                always {
                    nunit testResultsPattern: 'nbaDataTests/TestResults/*.xml'
                    jiraSendBuildInfo site: 'nba-data.atlassian.net'
                }
            }
               success {
                script {
                  println "All the tests passed."
                }
              }
              failure {
                println "There are some failing tests."
              }
	    }
	}
}

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
                }
               success {
                script {
                  println "All the tests passed."
                  emailext body: 'All tests are passing.', recipientProviders: [[$class: 'DevelopersRecipientProvider'], [$class: 'RequesterRecipientProvider']], subject: 'Test Results'
                  //If the branch is main, then deploy
                  if (env.BRANCH_NAME == 'main') {
                        println 'I only execute on the master branch'
                    }
                }
              }
              failure {
                println "There are some failing tests."
                emailext body: 'Build failed, please inspect jenkins console logs.', recipientProviders: [[$class: 'DevelopersRecipientProvider'], [$class: 'RequesterRecipientProvider']], subject: 'Test Results'

              }
            }
	    }
	}
}

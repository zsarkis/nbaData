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
                        // println 'I only execute on the master branch'
                        sh 'dotnet publish --configuration Release'
                        sh 'sudo python /home/ubuntu/deployBack.py'
                        sh 'sudo systemctl restart kestrel-nbaData.service'



                        sh 'sudo npm --prefix /var/lib/jenkins/workspace/NbaDataBuild_main/nba-data-front/ install --save'
                        sh 'sudo npm --prefix /var/lib/jenkins/workspace/NbaDataBuild_main/nba-data-front/ install react-scripts --save'
                        sh 'sudo chmod  -R 777 /var/lib/jenkins/workspace/NbaDataBuild_main/nba-data-front/'
                        // sh 'sudo npm install node-sass --save'
                        sh 'sudo chown -R $(whoami) /usr/local/lib/node_modules/'
                        sh 'sudo chmod -R 775 /usr/local/lib/node_modules/'
                        sleep(1)
                        dir("folder")
                        {
                          sh 'npm run build'
                        }
                        sh 'sudo python /home/ubuntu/deployFront.py'
                        sh 'sudo systemctl restart nginx'

                        sleep(10)
                        // sh 'curl --keepalive-time 500000 --location --request GET \'http://localhost:5000/api/v1/players\''
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

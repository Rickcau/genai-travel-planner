# ConcoleApp-Chat-Streaming
This is a simple console app that demostrates streaming data from an ASP.NET Core REST API.  You might think this is super simple but it's not.  ASP.NET can actaully cause the data to be buffered or converted to a string array, even when using IAsyncEnumerable.

# Streaming
Streaming of Chat Completitions is a very important concept, especially if it's an interaction between a use and an AI system.  The idea is you want to stream the results as the AI is completing the response.  When doing this, you can to string characters back to the client and display those in type writting style outpoint.  If you are using Text To Speach this it's important for that as well.

# Important Notes
When designing AI systems that are intended to interact with a human it's important to get some of the data back to the user as quick as possible and this is where streaming helps.  You also need to be very mindful of your prompts and spend time evaluating the prompts.  You could have a prompt that requires the LLM to spend time dealing with the logic to process the request i.e. building a JSON object that is large with lots of data.  When you have a situation that requires the LLM some time to process you can dive this into two tasks.  1) Task or Thread sends prompt to the LLM that is intented to render a response as quickly as possible while the 2nd task is spun up in the background processing the request that needs more time.

For example, let's say you want the LLM to perform some reasoning that per your testing regardless of the model it simply takes 5-10 sec to process and this is not acceptable to your users.  You have to come up with logic that allows that log running process to occur in the background.  What I like to do, is stream a summary back to the client and at the end of the stream I embed { 'JobId': '12345'}.  This tells the client there is more details they can retreive.  I expose a 2nd operation that allows the client to retreive those details using the JobID.

# Link to Video for the Example
![Video](https://www.youtube.com/watch?v=wcB8YM_g8k0)

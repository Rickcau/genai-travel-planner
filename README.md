# genai-travel-planner
This repo contains several examples of which I will list below.

## ConsoleApp-BagOfWords
This console app demostrats the Bag Of Words concept.

The code provided uses the FeaturizeText transformation in ML.NET to convert text data into numerical feature vectors. This transformation creates a Bag-of-Words (BoW) representation of the text, which is a common technique in natural language processing (NLP).

## ConsoleApp-Chat-Streaming
This is a simple console app that demostrates streaming data from an ASP.NET Core REST API. You might think this is super simple but it's not. ASP.NET can actaully cause the data to be buffered or converted to a string array, even when using IAsyncEnumerable.

## api-santize-validate
This API is used to allow vendors to submit an unstructured road trip, I leveage AI to check for harmful content, and santize as needed and convert in a structured format that can be later processed.

## api-roadtrip-input
This API is used to verify that the roadtrip data does not exceed any of our defined limits and if so it will refuse to process the data and send the client an error status.


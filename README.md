# ProductionPlan

Welcome to the challenge repository!

## Running on local
You can just clone or copy the complete repository, and run it via Visual Studio. Load the entire solution, and run it via Powerplant.API project.
When the application is running, you can use the swagger directly with this url:
http://localhost:8888/swagger/index.html

## API Reference

#### Get the production plan, based on the load and the merit order.

```http
  POST /productionplan
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `body` | `JSON` | **Required**. The JSON contains the load, the fuels costs, and the available powerplants|
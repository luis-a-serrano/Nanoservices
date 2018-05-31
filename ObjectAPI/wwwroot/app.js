/// <reference path="vendor/jquery-3.3.1.min.js" />
"use strict";

$(document).ready(async function () {
   // Properties Enum.
   const NodeProperties = Object.freeze({
      OwnCoefficients = "OwnCoefficients",
      OthersCoefficients = "OthersCoefficients",
      NeighborsList = "NeighborsList"
   });

   // Get the amount of nodes. Get the amount of coefficients.
   const numNodes = document.getElementById();//~~
   const numCoefficients = 0; //~~
   const testData = [[]]; //~~

   // Construct the nodes.
   const nodes = {};
   for (let l = 0; l < numNodes; l++) {

      let nodeId;
      do {
         nodeId = Math.random().toString(32).substring(2);
      } while (nodes.hasOwnProperty(nodeId));

      nodes[nodeId] = {
         data: testData[l],
         currentPromise: null
      };
   }

   // Properties Schema.
   const coefficientProperty = {
      Name: NodeProperties.OwnCoefficients,
      Schema: {
         Type: "Array"
      },
      Value: [],
      Writable: true
   };

   const neighborsProperty = {
      Name: NodeProperties.NeighborsList,
      Schema: {
         Type: "Array"
      },
      Value: [],
      Writable: true
   };

   // Add the properties to each node.
   const nodesIds = Object.getOwnPropertyNames(nodes);
   for (let nodeId in nodesIds) {
      const promises = [];
      const myNeighbors = nodesIds.filter(key => key != nodeId);

      promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, coefficientProperty, null, "json"));
      promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, neighborsProperty, null, "json"));

      for (let otherNode in myNeighbors) {
         // Property Schema.
         const coefficientPropertyForNeighbor = {
            Name: `${NodeProperties.OthersCoefficients}_${otherNode}`,
            Schema: {
               Type: "Array"
            },
            Value: [],
            Writable: true
         };

         promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, coefficientPropertyForNeighbor, null, "json"));
      }

      nodes[nodeId].currentPromise = Promise.all(promises);
   }

   // Wait for all the API calls to be finished.
   await Promise.all(nodesIds.map(nodeId => nodes[nodeId].currentPromise));

   const defaultCoefficients = [];
   defaultCoefficients.length = numCoefficients;
   defaultCoefficients.fill(0);


   for (let nodeId in nodesIds) {
      const initializeJson = {
         Address: "api/AdaptiveNetwork/initialize",//~~
         Payload: {
            Coefficients: {
               Ids: [].concat(
                  NodeProperties.OwnCoefficients,
                  nodesIds
                     .filter(key => key != nodeId)
                     .map(otherNode => `${NodeProperties.OthersCoefficients}_${otherNode}`)
               ),
               Values: defaultCoefficients
            }
         }
      };

      nodes[nodeId].currentPromise = $.post(`api/ObjectActor/${nodeId}/action`, initializeJson, null, "json")
   }

   await Promise.all(nodesIds.map(nodeId => nodes[nodeId].currentPromise));

   for (let currentIteration = 0; currentIteration < testData[0].length - numCoefficients; currentIteration++) { //~~

      for (let nodeId in nodesIds) {
         const calculateJson = {
            Address: "api/AdaptiveNetwork/learn",//~~
            Payload: {
               Iteration: {
                  Signal: nodes[nodeId].data[currentIteration + numCoefficients],
                  PastValues: nodes[nodeId].data.slice(currentIteration, currentIteration + numCoefficients).reverse()
               },
               OwnCoefficients: `#${NodeProperties.OwnCoefficients}#`,
               NeighborsCoefficients: nodesIds
                  .filter(key => key != nodeId)
                  .map(otherNode => `#${NodeProperties.OthersCoefficients}_${otherNode}#`)
            }
         };
         nodes[nodeId].currentPromise = $.post(`api/ObjectActor/${nodeId}/action`, calculateJson, null, "json");
      }

      await Promise.all(nodesIds.map(nodeId => nodes[nodeId].currentPromise));

      for (let nodeId in nodesIds) {
         const shareJson = {
            Address: "api/AdaptiveNetwork/share",//~~
            Payload: {
               TargetProperty: `${NodeProperties.OthersCoefficients}_${nodeId}`,
               Coefficients: `#${NodeProperties.OwnCoefficients}#`,
               Neighbors: `#${NodeProperties.NeighborsList}#`
            }
         };

         nodes[nodeId].currentPromise = $.post(`api/ObjectActor/${nodeId}/action`, shareJson, null, "json");
         await nodes[nodeId].currentPromise;
      }


   }
   



});
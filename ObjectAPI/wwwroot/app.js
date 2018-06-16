/// <reference path="vendor/jquery-3.3.1.min.js" />
/// <reference path="vendor/bootstrap.min.js" />
"use strict";

$(document).ready(function () {
   $("input[type='range'][lasr-bonded-with]").on("input", (event) => {
      const $_ = $(event.target);
      $(`#${$_.attr("lasr-bonded-with")}`).val($_.val());
   });

   $("input[type='range'][lasr-bonded-with]").trigger("input");

   $("#dataTabs").on("click", (event) => {
      event.preventDefault();

      const previousActive = $(event.currentTarget).children(".active").first();
      const newActive = $(event.target);

      // TODO: Remove the `false` when we implement graphs.
      if (false && !previousActive.is(newActive)) {
         previousActive.removeClass("active");
         newActive.addClass("active");
         
         $(`${previousActive.attr("lasr-bonded-with")}`).removeClass("show active");
         $(`${newActive.attr("lasr-bonded-with")}`).addClass("show active");
      }
   });

   $("#nodeTabs").on("click", (event) => {
      event.preventDefault();

      const previousActive = $(event.currentTarget).children(".active").first();
      const newActive = $(event.target);

      if (!previousActive.is(newActive)) {
         previousActive.removeClass("active");
         newActive.addClass("active");

         const allNodePanels = $(`#tabContents > [id] > :first-child`);
         allNodePanels.children(`:nth-child(${previousActive.index() + 1})`).removeClass("show active");
         allNodePanels.children(`:nth-child(${newActive.index() + 1})`).addClass("show active");
      }
   });

   $("#runDemo").on("click", async function () {
      // Properties Enum.
      const NodeProperties = Object.freeze({
         OwnCoefficients: "OwnCoefficients",
         OthersCoefficients: "OthersCoefficients",
         NeighborsList: "NeighborsList"
      });

      // Get the amount of nodes. Get the amount of coefficients.
      const numNodes = parseInt($("#numOfNodesDisplay").val(), 10);
      const numCoefficients = parseInt($("#numOfCoefficientsDisplay").val(), 10);
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

         promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, coefficientProperty, null, "json"));
         promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, neighborsProperty, null, "json"));

         const myNeighbors = nodesIds.filter(key => key != nodeId);
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

      const tabsContent = $("#tablesPanels > .tab-content");// Ad headers

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

         const estimations = await Promise.all(nodesIds.map(nodeId => nodes[nodeId].currentPromise));

         for (let l = estimations.length; l >= 1; l--) {
            const iterationRow = tabsContent.children(`.tab-pane:nth-child(${l}) > .table > tbody`).get(0).insertRow();

            iterationRow.insertCell().appendChild(document.createTextNode(currentIteration));
            iterationRow.insertCell().appendChild(document.createTextNode(estimations[l]));
            // TODO: Show the signal value. Because we iterate with `for...in` that means
            // that the order is not guranteed. Thus, we need to make a change to allow
            // a proper mapping for presenting that value.
         }

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
});

//$(document).ready(async function () {
//   // Properties Enum.
//   const NodeProperties = Object.freeze({
//      OwnCoefficients = "OwnCoefficients",
//      OthersCoefficients = "OthersCoefficients",
//      NeighborsList = "NeighborsList"
//   });

//   // Get the amount of nodes. Get the amount of coefficients.
//   const numNodes = document.getElementById();//~~
//   const numCoefficients = 0; //~~
//   const testData = [[]]; //~~

//   // Construct the nodes.
//   const nodes = {};
//   for (let l = 0; l < numNodes; l++) {

//      let nodeId;
//      do {
//         nodeId = Math.random().toString(32).substring(2);
//      } while (nodes.hasOwnProperty(nodeId));

//      nodes[nodeId] = {
//         data: testData[l],
//         currentPromise: null
//      };
//   }

//   // Properties Schema.
//   const coefficientProperty = {
//      Name: NodeProperties.OwnCoefficients,
//      Schema: {
//         Type: "Array"
//      },
//      Value: [],
//      Writable: true
//   };

//   const neighborsProperty = {
//      Name: NodeProperties.NeighborsList,
//      Schema: {
//         Type: "Array"
//      },
//      Value: [],
//      Writable: true
//   };

//   // Add the properties to each node.
//   const nodesIds = Object.getOwnPropertyNames(nodes);
//   for (let nodeId in nodesIds) {
//      const promises = [];

//      promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, coefficientProperty, null, "json"));
//      promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, neighborsProperty, null, "json"));

//      const myNeighbors = nodesIds.filter(key => key != nodeId);
//      for (let otherNode in myNeighbors) {
//         // Property Schema.
//         const coefficientPropertyForNeighbor = {
//            Name: `${NodeProperties.OthersCoefficients}_${otherNode}`,
//            Schema: {
//               Type: "Array"
//            },
//            Value: [],
//            Writable: true
//         };

//         promises.push($.post(`api/ObjectActor/${nodeId}/add-property`, coefficientPropertyForNeighbor, null, "json"));
//      }

//      nodes[nodeId].currentPromise = Promise.all(promises);
//   }

//   // Wait for all the API calls to be finished.
//   await Promise.all(nodesIds.map(nodeId => nodes[nodeId].currentPromise));

//   const defaultCoefficients = [];
//   defaultCoefficients.length = numCoefficients;
//   defaultCoefficients.fill(0);


//   for (let nodeId in nodesIds) {
//      const initializeJson = {
//         Address: "api/AdaptiveNetwork/initialize",//~~
//         Payload: {
//            Coefficients: {
//               Ids: [].concat(
//                  NodeProperties.OwnCoefficients,
//                  nodesIds
//                     .filter(key => key != nodeId)
//                     .map(otherNode => `${NodeProperties.OthersCoefficients}_${otherNode}`)
//               ),
//               Values: defaultCoefficients
//            }
//         }
//      };

//      nodes[nodeId].currentPromise = $.post(`api/ObjectActor/${nodeId}/action`, initializeJson, null, "json")
//   }

//   await Promise.all(nodesIds.map(nodeId => nodes[nodeId].currentPromise));

//   for (let currentIteration = 0; currentIteration < testData[0].length - numCoefficients; currentIteration++) { //~~

//      for (let nodeId in nodesIds) {
//         const calculateJson = {
//            Address: "api/AdaptiveNetwork/learn",//~~
//            Payload: {
//               Iteration: {
//                  Signal: nodes[nodeId].data[currentIteration + numCoefficients],
//                  PastValues: nodes[nodeId].data.slice(currentIteration, currentIteration + numCoefficients).reverse()
//               },
//               OwnCoefficients: `#${NodeProperties.OwnCoefficients}#`,
//               NeighborsCoefficients: nodesIds
//                  .filter(key => key != nodeId)
//                  .map(otherNode => `#${NodeProperties.OthersCoefficients}_${otherNode}#`)
//            }
//         };
//         nodes[nodeId].currentPromise = $.post(`api/ObjectActor/${nodeId}/action`, calculateJson, null, "json");
//      }

//      await Promise.all(nodesIds.map(nodeId => nodes[nodeId].currentPromise));

//      for (let nodeId in nodesIds) {
//         const shareJson = {
//            Address: "api/AdaptiveNetwork/share",//~~
//            Payload: {
//               TargetProperty: `${NodeProperties.OthersCoefficients}_${nodeId}`,
//               Coefficients: `#${NodeProperties.OwnCoefficients}#`,
//               Neighbors: `#${NodeProperties.NeighborsList}#`
//            }
//         };

//         nodes[nodeId].currentPromise = $.post(`api/ObjectActor/${nodeId}/action`, shareJson, null, "json");
//         await nodes[nodeId].currentPromise;
//      }


//   }
   



//});
Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Description
Imports BlazingPizzaVS

Namespace Controllers
    Public Class OrderDetailsController
        Inherits System.Web.Http.ApiController

        Private db As New BlazingPizzaEntities

        ' GET: api/OrderDetails
        Function GetOrderDetails() As IQueryable(Of OrderDetail)
            Return db.OrderDetails
        End Function

        ' GET: api/OrderDetails/5
        <ResponseType(GetType(OrderDetail))>
        Function GetOrderDetail(ByVal id As Decimal) As IHttpActionResult
            Dim orderDetail As OrderDetail = db.OrderDetails.Find(id)
            If IsNothing(orderDetail) Then
                Return NotFound()
            End If

            Return Ok(orderDetail)
        End Function

        ' PUT: api/OrderDetails/5
        <ResponseType(GetType(Void))>
        Function PutOrderDetail(ByVal id As Decimal, ByVal orderDetail As OrderDetail) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = orderDetail.Id Then
                Return BadRequest()
            End If

            db.Entry(orderDetail).State = EntityState.Modified

            Try
                db.SaveChanges()
            Catch ex As DbUpdateConcurrencyException
                If Not (OrderDetailExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/OrderDetails
        <ResponseType(GetType(OrderDetail))>
        Function PostOrderDetail(ByVal orderDetail As OrderDetail) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.OrderDetails.Add(orderDetail)

            Try
                db.SaveChanges()
            Catch ex As DbUpdateException
                If (OrderDetailExists(orderDetail.Id)) Then
                    Return Conflict()
                Else
                    Throw
                End If
            End Try

            Return CreatedAtRoute("DefaultApi", New With {.id = orderDetail.Id}, orderDetail)
        End Function

        ' DELETE: api/OrderDetails/5
        <ResponseType(GetType(OrderDetail))>
        Function DeleteOrderDetail(ByVal id As Decimal) As IHttpActionResult
            Dim orderDetail As OrderDetail = db.OrderDetails.Find(id)
            If IsNothing(orderDetail) Then
                Return NotFound()
            End If

            db.OrderDetails.Remove(orderDetail)
            db.SaveChanges()

            Return Ok(orderDetail)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function OrderDetailExists(ByVal id As Decimal) As Boolean
            Return db.OrderDetails.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace
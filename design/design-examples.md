


1) When account created, send email to customer

Definition: WHEN Account CREATED THEN @sendemail { to: Account.Email, title: "Thank you for joining"}

1b) When item added to basket, promote similar items 

Definition: WHEN ItemAddedToBasket THEN @promotesimilaritems { itemId: ItemAddedToBasket }

1) When "special account" created, send email  

Definition: WHEN Account CREATED WHERE Account.IsSpecial THEN SENDEMAIL {to: Account.Email, title: "Thank you for being our special customer"} 

3) When tier 2 customer places order, use express shipping

Definition: WHEN Order CREATED WHERE Order.Customer.Tier == 2 THEN UPDATE {set: Order.Delivery.IsExpress = true }

4) When Item depleted, place restock order
Definition: WHEN Stock updated WHERE Stock.Count == 0 THEN CREATE {type: RestockOrder, parameters: {stockId: Stock.Id, itemId: Stock.Item.Id, amount: 10}} 

4b) When restock order placed, send email to supplier
Definition: WHEN RestockOrder CREATED THEN SENDEMAIL {to: RETRIEVE SINGLE {Stock WHERE Id == StockId}.LastestRestock.Supplier.Email, title: "Can I get |RestockOrder.Amount| more of |RestockOrder.ItemId|"}

1) When orders in the last 30 days have a combined value of more than $100 upgrade account to tier 2 

Definition: WHEN OrderSubmitted WHERE OrderSubmitted.Order.Customer.Orders SUBWHERE Orders[o] SELECT o.Total SUM > 100 THEN UPDATE { set: Customer.Tier = 2 }


# $Text = 'SwN12345678'
# # base64 encode
# $EncodedText = [Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($Text))
# # base64 decode
# $DecodedText = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($EncodedText))
# # output
# Write-Host $EncodedText
# Write-Host $DecodedText


$Text = 'Server=discountdb;Port=5432;Database=DiscountDb;User Id=admin;Password=admin1234;'
# base64 encode
$EncodedText = [Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($Text))
# base64 decode
$DecodedText = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($EncodedText))
# output
Write-Host $EncodedText
Write-Host $DecodedText



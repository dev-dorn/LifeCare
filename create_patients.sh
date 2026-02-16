#!/bin/bash

FIRST_NAMES=("John" "Mary" "Peter" "Jane" "David" "Grace" "James" "Faith" "Daniel" "Lucy")
LAST_NAMES=("Kamau" "Wanjiru" "Ochieng" "Akinyi" "Mwangi" "Nyambura" "Otieno" "Wairimu")
COUNTIES=("Nairobi" "Kiambu" "Nakuru" "Mombasa" "Kisumu")
SUBCOUNTIES=("Westlands" "Dagoretti" "Kasarani" "Embakasi" "Langata")

for i in {3..52}; do  # Starting from 3 since you have 2 already
  FIRST=${FIRST_NAMES[$RANDOM % ${#FIRST_NAMES[@]}]}
  LAST=${LAST_NAMES[$RANDOM % ${#LAST_NAMES[@]}]}
  COUNTY=${COUNTIES[$RANDOM % ${#COUNTIES[@]}]}
  SUBCOUNTY=${SUBCOUNTIES[$RANDOM % ${#SUBCOUNTIES[@]}]}
  
  AGE=$((RANDOM % 56 + 5))
  YEAR=$((2026 - AGE))
  MONTH=$((RANDOM % 12 + 1))
  DAY=$((RANDOM % 28 + 1))
  DOB=$(printf "%04d-%02d-%02d" $YEAR $MONTH $DAY)
  
  SHIF=$(printf "SHF%09d" $i)
  PHONE=$(printf "071%07d" $i)
  
  if [ $AGE -ge 18 ]; then
    NATIONAL_ID="\"$(printf "%08d" $((30000000 + i)))\""
    GUARDIAN="null"
  else
    NATIONAL_ID="null"
    GUARDIAN='{
      "firstName": "Parent",
      "lastName": "'"$LAST"'",
      "relationship": "Mother",
      "phoneNumber": "0700'"$(printf "%06d" $i)"'"
    }'
  fi
  
  GENDER=$([ $((RANDOM % 2)) -eq 0 ] && echo "Male" || echo "Female")
  EMAIL=$(echo "$FIRST.$LAST@email.com" | tr '[:upper:]' '[:lower:]')
  
  echo "Creating patient $i: $FIRST $LAST (Age: $AGE)..."
  
  curl -X POST http://localhost:8080/api/Patients/register \
    -H "Content-Type: application/json" \
    -d '{
      "shifNumber": "'"$SHIF"'",
      "nationalId": '"$NATIONAL_ID"',
      "firstName": "'"$FIRST"'",
      "lastName": "'"$LAST"'",
      "dateOfBirth": "'"$DOB"'",
      "gender": "'"$GENDER"'",
      "phoneNumber": "'"$PHONE"'",
      "email": "'"$EMAIL"'",
      "county": "'"$COUNTY"'",
      "subCounty": "'"$SUBCOUNTY"'",
      "country": "Kenya",
      "zipCode": "00100",
      "guardian": '"$GUARDIAN"'
    }' 2>&1 | grep -q "success.*true" && echo " ✓" || echo " ✗ FAILED"
  
  sleep 0.1
done

echo ""
echo "Done! Check your patients:"
curl -s http://localhost:8080/api/Patients/search | jq '.data | length'
echo "patients created"

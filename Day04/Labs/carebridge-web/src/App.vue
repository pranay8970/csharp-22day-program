<script setup>
import { ref, onMounted } from 'vue'

// Reactive array that will hold patients.
const patients = ref([])

// Runs automatically when page loads.
onMounted(async () => {

  // Call ASP.NET Core API. – Change the port
  const response =
    await fetch('https://localhost:7130/api/patients')

  // Convert JSON into JavaScript objects.
  patients.value =
    await response.json()

})
</script>

<template>

  <h1>CareBridge Patients</h1>

  <table border="1">

    <tr>
      <th>Patient Id</th>
      <th>Full Name</th>
      <th>City</th>
    </tr>

    <!-- Loop through all patients -->

    <tr
      v-for="p in patients"
      :key="p.patientId">

      <td>{{ p.patientId }}</td>
      <td>{{ p.fullName }}</td>
      <td>{{ p.city }}</td>

    </tr>

  </table>

</template>

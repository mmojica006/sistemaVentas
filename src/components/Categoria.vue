<template>
  <v-layout align-start>
    <v-flex>
      <v-data-table
        :headers="headers"
        :items="categorias"
        :search="search"
        sort-by="calories"
        class="elevation-1"
      >
        <template v-slot:top>
          <v-toolbar flat color="white">
            <v-toolbar-title>Categorías</v-toolbar-title>
            <v-divider class="mx-4" inset vertical></v-divider>
            <v-spacer></v-spacer>
            <v-text-field
              class="text-xs-center"
              v-model="search"
              append-icon="search"
              label="Búsqueda"
              single-line
              hide-details
            ></v-text-field>
            <v-spacer></v-spacer>
            <!--Modal-->
            <v-dialog v-model="dialog" max-width="500px">
              <template v-slot:activator="{ on }">
                <v-btn color="primary" dark class="mb-2" v-on="on">Nuevo</v-btn>
              </template>
              <v-card>
                <v-card-title>
                  <span class="headline">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col cols="12" sm="12" md="12">
                        <v-text-field v-model="nombre" label="Nombre"></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="12" md="12">
                        <v-text-field v-model="descripcion" label="Descripcion"></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="12" md="12" v-show="valida">
                        <div class="red--text" v-for="v in validaMensaje" :key="v" v-text="v"></div>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="close">Cancelar</v-btn>
                  <v-btn color="blue darken-1" text @click="guardar">Guardar</v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
            <!--Modal-->
          </v-toolbar>
        </template>
        <template v-slot:item.opciones="{ item }">
          <v-icon small class="mr-2" @click="editItem(item)">edit</v-icon>
          <v-icon small @click="deleteItem(item)">delete</v-icon>
        </template>
        <template v-slot:item.condicion="{ item }">
          <v-card-text v-if="item.condicion" class="blue--text">Activo</v-card-text>
          <v-card-text v-if="!item.condicion" class="red--text">Inactivo</v-card-text>
        </template>
        <template v-slot:no-data>
          <v-btn color="primary">Reset</v-btn>
        </template>
      </v-data-table>
    </v-flex>
  </v-layout>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      categorias: [],
      dialog: false,
      headers: [
        { text: "Nombre", value: "nombre" },
        { text: "Descripción", value: "descripcion", sortable: false },
        { text: "Estado", value: "condicion", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false }
      ],
      search: "",

      editedIndex: -1,
      editedItem: {
        name: "",
        calories: 0,
        fat: 0,
        carbs: 0,
        protein: 0
      },
      id: "",
      nombre: "",
      descripcion: "",
      valida: 0,
      validaMensaje: []
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1
        ? "Nueva Categoría"
        : "Actualizar categoría";
    }
  },

  watch: {
    dialog(val) {
      val || this.close();
    }
  },

  created() {
    this.listar();
  },

  methods: {
    listar() {
      let me = this; //Para poder utilizar el list dentro de cualquier llamado accion
      axios
        .get("api/Categorias/Listar")
        .then(function(response) {
          // console.log(response);
          me.categorias = response.data;
        })
        .catch(function(error) {
          console.log(error);
        });
    },

    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      const index = this.desserts.indexOf(item);
      confirm("Are you sure you want to delete this item?") &&
        this.desserts.splice(index, 1);
    },

    close() {
      this.dialog = false;
    },
    limpiar() {
      this.id = "";
      this.nombre = "";
      this.descripcion = "";
    },
    guardar() {
      if (this.validar()) {
        return;
      }

      if (this.editedIndex > -1) {
        //Código para editar
      } else {
        //codigo para guardar
        let me = this;
        axios
          .post("api/Categorias/crear", {
            nombre: me.nombre,
            descripcion: me.descripcion
          })
          .then(function(response) {
            me.close();
            me.listar();
            me.limpiar();
          })
          .catch(function(error) {
            console.log(error);
          });
      }
    },
    validar() {
      this.valida = 0;
      this.validaMensaje = [];
      if (this.nombre.length < 3 || this.nombre.length > 50) {
        this.validaMensaje.push(
          "El nombre debe tener mas de 3 caracteres y menos de 50 caracteres"
        );
      }
      if (this.validaMensaje.length) {
        this.valida = 1;
      }
      return this.valida;
    }
  }
};
</script>
